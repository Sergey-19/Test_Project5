using Avalonia;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Tool;
using EQX.FlowUILibraryWPF.Algorithms.Defines;
using EQX.FlowUILibraryWPF.Algorithms.Models;
using EQX.FlowUILibraryWPF.Models;
using EQX.FlowUILibraryWPF.MVVM.Views;
using EQX.Vision.Tool;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCvSharp;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static OpenCvSharp.Stitcher;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FlowViewModel : ObservableObject
    {
        #region Properties
        [JsonProperty]
        public ObservableCollection<ToolViewModel> ToolList { get; }
        [JsonProperty]
        public ObservableCollection<ConnectionData> ConnectionPathList { get; }
        [JsonProperty]
        public Dictionary<string, Type> dataTypeDictionary = new Dictionary<string, Type>();
        public string keyTempInput;
        public string keyTempOutput;
        public ConnectionData TemporaryConnectionPath { get; set; }
        [JsonProperty]
        public string FlowName
        {
            get
            {
                return _flowName;
            }
            set
            {
                _flowName = value;
                OnPropertyChanged();
            }
        }
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
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }
        #endregion

        #region Constructor
        public FlowViewModel(string name)
        {
            FlowName = name;
            ToolList = new ObservableCollection<ToolViewModel>();
            ConnectionPathList = new ObservableCollection<ConnectionData>();

            DragDeltaCommand = new RelayCommand<DragDeltaEventArgs>(DragDelta);
            OnFlowMouseMoveCommand = new RelayCommand<MouseEventArgs>(OnFlowMouseMove);
            OnFlowMouseDownCommand = new RelayCommand<MouseEventArgs>(OnFlowMouseDown);
            RunFlowCommand = new RelayCommand(() =>
            {
                RunFlow(null, EventArgs.Empty);
            });
            AddTypeTool();
        }

        private void AddTypeTool()
        {
            dataTypeDictionary.Add("ImageMat", typeof(Mat));
            dataTypeDictionary.Add("Center", typeof(Point2f));
            dataTypeDictionary.Add("Radius", typeof(float));
            dataTypeDictionary.Add("LocationX", typeof(float));
            dataTypeDictionary.Add("LocationY", typeof(float));
        }

        [JsonConstructor]
        public FlowViewModel(string flowName, ObservableCollection<ToolViewModel> toolList, ObservableCollection<ConnectionData> connectionPathList)
        {
            FlowName = flowName;
            ToolList = toolList;
            ConnectionPathList = connectionPathList;

            DragDeltaCommand = new RelayCommand<DragDeltaEventArgs>(DragDelta);
            OnFlowMouseMoveCommand = new RelayCommand<MouseEventArgs>(OnFlowMouseMove);
            OnFlowMouseDownCommand = new RelayCommand<MouseEventArgs>(OnFlowMouseDown);

            RunFlowCommand = new RelayCommand(() =>
            {
                RunFlow(null, EventArgs.Empty);
            });

            foreach (var tool in ToolList)
            {
                tool.RunEvent += RunFlow;
                tool.OnToolMouseMoveEvent += OnToolMouseMove;
                tool.PointInputMouseDownEvent += ToolPointInputMouseDown;
                tool.PointInputMouseMoveEvent += ToolPointInputMouseMove;

                tool.PointOutputMouseDownEvent += ToolPointOutputMouseDown;
                tool.PointOutputMouseMoveEvent += ToolPointOutputMouseMove;

                tool.DeleteToolEvent += DeleteTool;
            }
            foreach (var path in ConnectionPathList)
            {
                path.DeletePathEvent += DeletePathEventHandler;
                path.OriginTool = ToolList.FirstOrDefault(t => t.Id == path.OriginTool.Id);
                path.TargetTool = ToolList.FirstOrDefault(t => t.Id == path.TargetTool.Id);
            }
        }
        #endregion

        #region Commands
        public ICommand DragDeltaCommand { get; set; }
        public ICommand OnFlowMouseMoveCommand { get; set; }
        public ICommand OnFlowMouseDownCommand { get; set; }
        public ICommand RunFlowCommand { get; set; }
        #endregion

        #region Private Function
        private void AddToolToFlow(IVisionTool visionTool)
        {
            ToolViewModel tool = new ToolViewModel(visionTool);
            tool.Id = _id;
            foreach (var item in visionTool.Inputs.Keys)
            {
                foreach (var item1 in visionTool.Outputs.Keys)
                {
                    Port p1 = new Port(tool.Id, item, PortType.Input);
                    Port p2 = new Port(tool.Id, item1, PortType.Output);
                    if (Ports.Contains(p1) == false) Ports.Add(p1);
                    if (Ports.Contains(p2) == false) Ports.Add(p2);
                    int i1 = GetIndex(p1);
                    int i2 = GetIndex(p2);
                    Graph.AddEdge(Ports[i1], Ports[i2]);
                }
            }
            tool.RunEvent += RunFlow;
            tool.OnToolMouseMoveEvent += OnToolMouseMove;
            tool.PointInputMouseDownEvent += ToolPointInputMouseDown;
            tool.PointInputMouseMoveEvent += ToolPointInputMouseMove;
            tool.PointOutputMouseDownEvent += ToolPointOutputMouseDown;
            tool.PointOutputMouseMoveEvent += ToolPointOutputMouseMove;
            tool.DeleteToolEvent += DeleteTool;

            ToolList.Add(tool);
            OnPropertyChanged(nameof(ToolList));
        }
        private void DragDelta(DragDeltaEventArgs e)
        {
            var thumb = e.Source as Thumb;
            int count = VisualTreeHelper.GetChildrenCount(thumb);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(thumb, i);
                if (child != null && child is ToolView)
                {
                    ((child as ToolView).DataContext as ToolViewModel).Position.Left = Canvas.GetLeft(thumb) + e.HorizontalChange;
                    ((child as ToolView).DataContext as ToolViewModel).Position.Top = Canvas.GetTop(thumb) + e.VerticalChange;
                }
                ConnectionPathList.Where(t => t.OriginTool == (child as ToolView).DataContext as ToolViewModel || t.TargetTool == (child as ToolView).DataContext as ToolViewModel).ToList().ForEach(t => t.GenerateLinkPath(1));
            }
        }
        private void OnFlowMouseMove(MouseEventArgs e)
        {
            if (TemporaryConnectionPath == null) return;
            if (_isConnecting == false)
            {
                e.Handled = true;
                return;
            }
            else
            {
                System.Windows.Point currentPos = e.GetPosition(e.Source as Canvas);
                if (TemporaryConnectionPath.TargetTool == null)
                {
                    TemporaryConnectionPath.GenerateLinkPath(TemporaryConnectionPath.StartPoint, currentPos, 1);
                    OnPropertyChanged(nameof(TemporaryConnectionPath));
                }
                if (TemporaryConnectionPath.OriginTool == null)
                {
                    TemporaryConnectionPath.GenerateLinkPath(currentPos, TemporaryConnectionPath.EndPoint, 1);
                    OnPropertyChanged(nameof(TemporaryConnectionPath));
                }
            }
        }
        private void OnFlowMouseDown(MouseEventArgs e)
        {
            if (TemporaryConnectionPath == null) return;
            if (TemporaryConnectionPath.GeometryPathData == null) return;

            TemporaryConnectionPath.GeometryPathData = null;
            TemporaryConnectionPath = null;
            _isConnecting = false;
        }
        private void OnToolMouseMove(object sender, EventArgs e)
        {
            if (_isConnecting == false) return;

            if (TemporaryConnectionPath == null) return;

            if (TemporaryConnectionPath.TargetTool == null && TemporaryConnectionPath.OriginTool != null)
            {
                if (TemporaryConnectionPath.OriginTool != (sender as ToolViewModel))
                {
                    TemporaryConnectionPath.GenerateLinkPath(TemporaryConnectionPath.StartPoint, (sender as ToolViewModel).GetCenterInputPoint(1), 1);
                }
            }
            else if (TemporaryConnectionPath.OriginTool == null && TemporaryConnectionPath.TargetTool != null)
            {
                if (TemporaryConnectionPath.TargetTool != (sender as ToolViewModel))
                {
                    TemporaryConnectionPath.GenerateLinkPath((sender as ToolViewModel).GetCenterOutputPoint(1), TemporaryConnectionPath.EndPoint, 1);
                }
            }
        }
        private void ToolPointInputMouseDown(object sender, EventArgs e)
        {
            if (TemporaryConnectionPath == null)
            {
                TemporaryConnectionPath = new ConnectionData();
                TemporaryConnectionPath.DeletePathEvent += DeletePathEventHandler;
            }

            string key = ((e as MouseButtonEventArgs).Source as Ellipse).Tag.ToString();
            keyTempInput = key;

            if (_isConnecting)
            {
                if (TemporaryConnectionPath.OriginTool != null)
                {
                    if ((sender as ToolViewModel) == TemporaryConnectionPath.OriginTool || dataTypeDictionary[key] != dataTypeDictionary[keyTempOutput])
                    {
                        _isConnecting = false;
                        TemporaryConnectionPath.Dispose();
                        return;
                    }
                    TemporaryConnectionPath.TargetTool = (sender as ToolViewModel);
                    TemporaryConnectionPath.KeyInputTargetTool = key;

                    ConnectionPathList.Add(TemporaryConnectionPath.Clone());
                    if (TemporaryConnectionPath.TargetTool != null && TemporaryConnectionPath.OriginTool != null)
                    {
                        if (_isConnecting)
                        {
                            Port p1 = new Port(TemporaryConnectionPath.OriginTool.Id, TemporaryConnectionPath.KeyOutputOriginTool, PortType.Output);
                            Port p2 = new Port(TemporaryConnectionPath.TargetTool.Id, TemporaryConnectionPath.KeyInputTargetTool, PortType.Input);
                            int i1 = GetIndex(p1);
                            int i2 = GetIndex(p2);
                            var existPaths = ConnectionPathList.Where(p => p.TargetTool == TemporaryConnectionPath.TargetTool && p.KeyInputTargetTool == TemporaryConnectionPath.KeyInputTargetTool).ToList();
                            if (existPaths != null)
                            {
                                foreach (var existPath in existPaths)
                                {
                                    ConnectionPathList.Remove(existPath);
                                    if (i1 != -1 && i2 != -1)
                                        Graph.RemoveEdge(Ports[i1], Ports[i2]);

                                }
                            }
                            if (i1 != -1 && i2 != -1)
                                Graph.AddEdge(Ports[i1], Ports[i2]);

                            if (Graph.HasCycle())
                            {
                                if (i1 != -1 && i2 != -1)
                                    Graph.RemoveEdge(Ports[i1], Ports[i2]);
                                TemporaryConnectionPath.Dispose();
                                _isConnecting = false;
                                return;
                            }

                            ConnectionPathList.Add(TemporaryConnectionPath.Clone());
                            _isConnecting = false;
                        }
                    }
                    TemporaryConnectionPath.Dispose();
                    _isConnecting = false;

                }
            }
            else
            {
                _isConnecting = true;
                TemporaryConnectionPath.TargetTool = (sender as ToolViewModel);
                TemporaryConnectionPath.KeyInputTargetTool = key;
            }
        }
        private Graph<Port> Graph = new Graph<Port>();
        private List<Port> Ports = new List<Port>();
        private int GetIndex(Port port)
        {
            for (int i = 0; i < Ports.Count; ++i)
            {
                if (Ports[i].ToolId == port.ToolId
                    && Ports[i].KeyPort == port.KeyPort
                    && Ports[i].PortType == port.PortType) return i;
            }
            return -1;
        }

        private void ToolPointInputMouseMove(object sender, EventArgs e)
        {
            if (TemporaryConnectionPath == null) return;
            if (_isConnecting == false) return;

            string key = ((e as MouseEventArgs).Source as Ellipse).Tag.ToString();

            if (TemporaryConnectionPath.OriginTool != null)
            {
                var tool = (sender as ToolViewModel);
                System.Windows.Point endPoint = tool.GetCenterInputPoint(tool.VisionTool.Inputs.Keys.ToList().IndexOf(key) + 1);

                TemporaryConnectionPath.GenerateLinkPath(TemporaryConnectionPath.StartPoint, endPoint, 1);
            }
        }
        private void ToolPointOutputMouseDown(object sender, EventArgs e)
        {
            if (TemporaryConnectionPath == null)
            {
                TemporaryConnectionPath = new ConnectionData();
                TemporaryConnectionPath.DeletePathEvent += DeletePathEventHandler;
            }
            string key = ((e as MouseButtonEventArgs).Source as Ellipse).Tag.ToString();
            keyTempOutput = key;

            if (_isConnecting)
            {
                if (TemporaryConnectionPath.TargetTool != null)
                {
                    if ((sender as ToolViewModel) == TemporaryConnectionPath.TargetTool || dataTypeDictionary[key] != dataTypeDictionary[keyTempInput])
                    {
                        _isConnecting = false;
                        TemporaryConnectionPath.GeometryPathData = null;
                        TemporaryConnectionPath.Dispose();
                        return;
                    }
                    TemporaryConnectionPath.OriginTool = (sender as ToolViewModel);
                    TemporaryConnectionPath.KeyOutputOriginTool = key;

                    ConnectionPathList.Add(TemporaryConnectionPath.Clone());
                    if (TemporaryConnectionPath.TargetTool != null && TemporaryConnectionPath.OriginTool != null)
                    {
                        if (_isConnecting)
                        {
                            Port p1 = new Port(TemporaryConnectionPath.OriginTool.Id, TemporaryConnectionPath.KeyOutputOriginTool, PortType.Output);
                            Port p2 = new Port(TemporaryConnectionPath.TargetTool.Id, TemporaryConnectionPath.KeyInputTargetTool, PortType.Input);
                            int i1 = GetIndex(p1);
                            int i2 = GetIndex(p2);
                            var existPaths = ConnectionPathList.Where(p => p.TargetTool == TemporaryConnectionPath.TargetTool && p.KeyInputTargetTool == TemporaryConnectionPath.KeyInputTargetTool).ToList();
                            if (existPaths != null)
                            {
                                foreach (var existPath in existPaths)
                                {
                                    ConnectionPathList.Remove(existPath);
                                    if (i1 != -1 && i2 != -1)
                                        Graph.RemoveEdge(Ports[i1], Ports[i2]);
                                }
                            }
                            if (i1 != -1 && i2 != -1)
                            {
                                Graph.AddEdge(Ports[i1], Ports[i2]);
                            }

                            if (Graph.HasCycle())
                            {
                                if (i1 != -1 && i2 != -1)
                                    Graph.RemoveEdge(Ports[i1], Ports[i2]);
                                TemporaryConnectionPath.Dispose();
                                _isConnecting = false;
                                return;
                            }
                            ConnectionPathList.Add(TemporaryConnectionPath.Clone());
                            _isConnecting = false;
                        }
                    }
                    TemporaryConnectionPath.Dispose();
                    _isConnecting = false;
                }
            }
            else
            {
                _isConnecting = true;
                TemporaryConnectionPath.OriginTool = (sender as ToolViewModel);
                TemporaryConnectionPath.KeyOutputOriginTool = key;
            }
        }
        private void ToolPointOutputMouseMove(object sender, EventArgs e)
        {
            if (TemporaryConnectionPath == null) return;
            if (_isConnecting == false) return;

            string key = ((e as MouseEventArgs).Source as Ellipse).Tag.ToString();

            if (TemporaryConnectionPath.TargetTool != null)
            {
                var tool = (sender as ToolViewModel);
                System.Windows.Point startPoint = tool.GetCenterOutputPoint(tool.VisionTool.Outputs.Keys.ToList().IndexOf(key) + 1);

                TemporaryConnectionPath.GenerateLinkPath(startPoint, TemporaryConnectionPath.EndPoint, 1);
            }
        }
        private void DeleteTool(object sender, EventArgs e)
        {
            ToolList.Remove(sender as ToolViewModel);

            var paths = ConnectionPathList.Where(p => p.OriginTool == (sender as ToolViewModel) || p.TargetTool == (sender as ToolViewModel)).ToList();
            foreach (var path in paths)
            {
                ConnectionPathList.Remove(path);
            }
            if (TemporaryConnectionPath != null)
            {
                TemporaryConnectionPath.GeometryPathData = null;
                TemporaryConnectionPath.Dispose();
            }
        }
        private void RunFlow(object sender, EventArgs e)
        {
            var watch = new Stopwatch();
            watch.Start();
            foreach (var tool in ToolList)
            {
                tool.VisionTool.RunAsync(500);
            }
            while (true)
            {
                List<ToolViewModel> listNotDone = ToolList.Where(t => t.VisionTool.State != Core.Common.ERunState.Done).ToList();
                if (listNotDone.Count <= 0)
                {
                    break;
                }
                else
                {
                    foreach (var tool in listNotDone)
                    {
                        // Get all relation of the current not run tool
                        var allDependency = ConnectionPathList.Where(path => path.TargetTool == tool).ToList();

                        if (allDependency.Any(path => path.OriginTool.VisionTool.State != Core.Common.ERunState.Done)) continue;

                        foreach (var dependency in allDependency)
                        {
                            string keyOutput = dependency.KeyOutputOriginTool;
                            string keyInput = dependency.KeyInputTargetTool;

                            if (tool.VisionTool.Inputs[keyInput] != null) continue;

                            if (keyInput == "ImageMat" && keyOutput == "ImageMat")
                            {
                                if (dependency.OriginTool.VisionTool.Outputs[keyOutput] != null)
                                    tool.VisionTool.Inputs[keyInput] = (dependency.OriginTool.VisionTool.Outputs[keyOutput] as Mat).Clone();
                            }
                            else
                            {
                                tool.VisionTool.Inputs[keyInput] = dependency.OriginTool.VisionTool.Outputs[keyOutput];
                            }
                        }
                    }
                }

                Thread.Sleep(2);
            }
            watch.Stop();
            ExecuteTime = watch.ElapsedMilliseconds;
        }
        private void DeletePathEventHandler(object sender, EventArgs e)
        {
            ConnectionPathList.Remove(sender as ConnectionData);
        }
        public void AddTool(string toolType)
        {
            switch (toolType)
            {
                case "LoadImage":
                    AddToolToFlow(new LoadImageTool() { Id = _id });

                    break;
                case "ConvertColor":
                    AddToolToFlow(new ConvertColorTool() { Id = _id });
                    break;

                case "Bitwise":
                    AddToolToFlow(new BitwiseTool() { Id = _id });
                    break;
                case "Threshold":
                    AddToolToFlow(new ThresholdTool() { Id = _id });
                    break;
                case "AdaptiveThreshold":
                    AddToolToFlow(new AdaptiveThresholdTool() { Id = _id });
                    break;
                case "InRange":
                    AddToolToFlow(new InRangeTool() { Id = _id });
                    break;
                case "Smoothing":
                    AddToolToFlow(new SmoothingTool() { Id = _id });
                    break;

                case "HoughCircle":
                    AddToolToFlow(new HoughCircleTool() { Id = _id });
                    break;
                case "FindCircle":
                    AddToolToFlow(new FindCircleTool() { Id = _id });
                    break;

                case "CreateCircle":
                    AddToolToFlow(new CreateCircleTool() { Id = _id });
                    break;

                case "TemplateMatching":
                    AddToolToFlow(new TemplateMatchingTool() { Id = _id });
                    break;
            }
        }

        #endregion

        #region Privates
        private string _flowName;
        private int _id => ToolList.Count < 1 ? 0 : ToolList.Last().Id + 1;
        private long _executeTime;
        private string _filePath;
        #endregion

        #region Public Function
        public void SaveAs()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            settings.Converters.Add(new StringEnumConverter());
            string serializationString = JsonConvert.SerializeObject(this, Formatting.Indented, settings);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = FlowName;
            saveFileDialog.Filter = "Text Files |*.txt";
            saveFileDialog.DefaultExt = "txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllTextAsync(saveFileDialog.FileName, serializationString);
            }
        }
        #endregion
        private bool _isConnecting = false;
    }
}
