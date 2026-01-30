using EQX.Core.Common;
using Microsoft.Win32;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using OpenCvSharp;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using log4net;
using EQX.Core.Vision.Algorithms;

namespace EQX.Vision.Algorithms
{
    public class VisionFlow : ObservableObject, IVisionFlow, ILogable
    {
        public event VisionFlowRunFinishedHandler VisionFlowRunFinished;
        public event EventHandler SaveVisionFlowHandler;

        public int Id { get; init; }
        public string Name { get; init; }
        [JsonIgnore]
        public ERunState State { get; internal set; }
        public double PixelSize { get; set; } = 1.0;
        public ObservableCollection<IVisionTool> VisionTools { get; }
        public ObservableCollection<VisionToolConnection> VisionToolConnections { get; }
        public VisionFlowDescription FlowDescription { get; set; }
        [JsonIgnore]
        public IObjectCollection Outputs => VisionTools.FirstOrDefault(vt => vt is OutputTool,new OutputTool()).Inputs;
        [JsonIgnore]
        public long ExecuteTime
        {
            get
            {
                return _executeTime;
            }
            set
            {
                _executeTime = value;
                OnPropertyChanged();
            }
        }

        public ILog Log => LogManager.GetLogger(Name);

        public VisionFlow(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Argument null or empty");

            Name = name;

            VisionTools = new ObservableCollection<IVisionTool>();
            VisionToolConnections = new ObservableCollection<VisionToolConnection>();
            FlowDescription = new VisionFlowDescription(this);

            State = ERunState.Idle;
            VisionToolConnections.CollectionChanged += VisionToolConnectionsChanged;
        }

        [JsonConstructor]
        public VisionFlow(int id, string name, ObservableCollection<IVisionTool> visionTools,
            ObservableCollection<VisionToolConnection> visionToolConnections,
            VisionFlowDescription flowDescription)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Argument null or empty");

            Id = id;
            Name = name;
            VisionTools = visionTools;
            VisionToolConnections = visionToolConnections;
            FlowDescription = flowDescription;
            VisionToolConnections.CollectionChanged += VisionToolConnectionsChanged;
        }

        public async Task RunAsync(int timeoutMs)
        {
            // Prevent multiple run
            if (State == ERunState.Running) return;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            State = ERunState.Running;

            foreach (var visionTool in VisionTools)
            {
                ((VisionToolBase)visionTool).PixelSize = PixelSize;
            }

            if (isFirstRun)
            {
                SubscribeToolRunFinish();
                isFirstRun = false;
            }

            List<Task> visionToolTasks = new List<Task>();
            foreach (var tool in VisionTools)
            {
                ((VisionToolBase)tool).State = ERunState.Running;
                visionToolTasks.Add(tool.RunAsync(timeoutMs));
            }

            await Task.WhenAny(Task.WhenAll(visionToolTasks), Task.Delay(timeoutMs));
            
            if (stopWatch.ElapsedMilliseconds > timeoutMs)
            {
                if (VisionTools.Any(tool => string.IsNullOrEmpty(tool.ErrorMessage)))
                {
                    Log.Error($"{Name} run timeout : {VisionTools.First(tool => string.IsNullOrEmpty(tool.ErrorMessage) == false)?.Name}");
                }
                Log.Error($"{Name} run timeout");

                State = ERunState.RuntimeError;
            }
            else
            {
                Log.Debug($"{Name} run finished success");

                State = ERunState.Done;
            }

            VisionFlowRunFinished?.Invoke(this,Outputs);
            stopWatch.Stop();
            ExecuteTime = stopWatch.ElapsedMilliseconds;
            Log.Debug($"{Name} execute time : {ExecuteTime} ms");
        }

        private void VisionToolConnectionsChanged(object sender, EventArgs e)
        {
            FlowDescription.VisiontoolConnectionsChanged((ObservableCollection<VisionToolConnection>)sender,e);

            SubscribeToolRunFinish();
        }

        private bool isFirstRun = true;

        private void SubscribeToolRunFinish()
        {
            foreach (IVisionTool tool in VisionTools)
            {
                tool.ToolRunFinished = (errorMessage, outputs) =>
                {
                    if (errorMessage != string.Empty) return;

                    VisionToolConnections.Where(vtc => vtc.OriginToolId == tool.Id).ToList().ForEach(vtc =>
                    {
                        foreach (IVisionTool targetTool in VisionTools)
                        {
                            if (targetTool.Id == vtc.TargetToolId)
                            {
                                if (vtc.OriginKey == "ImageMat" & vtc.TargetKey == "ImageMat")
                                    targetTool.Inputs[vtc.TargetKey] = ((Mat)outputs[vtc.OriginKey]!).Clone();
                                else
                                {
                                    targetTool.Inputs[vtc.TargetKey] = outputs[vtc.OriginKey]!;
                                }
                            }
                        }
                    });
                };
            }
        }

        public void Save()
        {
            SaveVisionFlowHandler?.Invoke(this, EventArgs.Empty);
        }

        private long _executeTime;
    }
}
