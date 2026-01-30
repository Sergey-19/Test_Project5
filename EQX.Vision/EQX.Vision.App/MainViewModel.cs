using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Algorithms;
using EQX.Core.Vision.Grabber;
using EQX.Vision.Algorithms;
using EQX.Vision.Grabber;
using EQX.Vision.Grabber.Helpers;
using EQX.Vision.Grabber.Hikrobot;
using EQX.VisionUI.WPF.MVVM.ViewModels;
using Microsoft.Win32;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using OpenCvSharp;
using System.Windows.Input;
using System.Xml.Linq;
using System.IO;
using System.Windows;

namespace EQX.Vision.App
{
    public class MainViewModel : ObservableObject
    {
        private Mat _image;

        public Mat Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); }
        }
        public ICommand GrabCommand { get; }
        public ICommand ContinuousCommand { get; }

        public ICommand DisconnectCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand StopContinuousCommand { get; }
        public MainViewModel()
        {
            //CameraHikrobotGigEv2 camera1 = new CameraHikrobotGigEv2("169.254.5.98", "camera1");
            //camera1.ContinuousImageGrabbed += Camera1_ContinuousImageGrabbed;
            //GrabCommand = new RelayCommand(() =>
            //{
            //    if (camera1.IsConnected)
            //    {
            //        GrabData grabData = camera1.GrabSingle();
            //        Image = GrabberHelpers.ToMat(grabData);
            //    }
            //});

            //ContinuousCommand = new RelayCommand(() =>
            //{
            //    if(camera1.IsConnected) 
            //    camera1.ContinuousImageGrabStart();
            //});
            //StopContinuousCommand = new RelayCommand(() =>
            //{
            //    camera1.ContinuousImageGrabStop();
            //});
            //DisconnectCommand = new RelayCommand(() =>
            //{
            //    if(camera1.IsConnected)
            //    camera1.Disconnect();
            //});
            //ConnectCommand = new RelayCommand(() =>
            //{
            //    camera1.Connect();
            //});
        }

        //private void Camera1_ContinuousImageGrabbed(object? sender, GrabData e)
        //{
        //    Image = e.ToMat();
        //}

        private VisionTeachingViewModel visionTechingVM;
        public VisionTeachingViewModel VisionTeachingVM
        {
            get
            {
                IVisionFlowRepository visionFlowRepository = new VisionFlowRepository();
                visionFlowRepository.Init(new List<IVisionFlow> { new VisionFlow("ALIGN") });

                ((VisionFlow)visionFlowRepository.GetAll().First()).SaveVisionFlowHandler += SaveVisionFlowHandler;
                ICamera camera = new SimulationCamera("D:\\UTGAutoLoadUnload\\Vision\\DataSimulation");
                VisionTeachingViewModel vm = new VisionTeachingViewModel(new VisionToolRepository("ToolList.json"), visionFlowRepository);
                vm.AddToolToCurrentFlow(new GrabTool() { Camera = camera }, new Algorithms.Point(0, 0));
                vm.LoadVisionFlowHandler += (s, e) =>
                {
                    try
                    {

                        OpenFileDialog openfileDialog = new OpenFileDialog();
                        if (openfileDialog.ShowDialog() == true)
                        {
                            if (openfileDialog.FileNames.Length > 1) return;
                            string serializationString = File.ReadAllText(openfileDialog.FileName);

                            var settings = new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            };
                            VisionFlow flow = JsonConvert.DeserializeObject<VisionFlow>(serializationString, settings);
                            flow.SaveVisionFlowHandler += new EventHandler(SaveVisionFlowHandler);
                            flow.FlowDescription = new VisionFlowDescription(flow, flow.FlowDescription.VisionToolDescriptions);
                            vm.CurrentFlow = flow;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };

                return visionTechingVM ?? (visionTechingVM = vm);
            }
        }

        private void SaveVisionFlowHandler(object? sender, EventArgs e)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new MatConverter());
            string serializationString = JsonConvert.SerializeObject((sender as VisionFlow), Formatting.Indented, settings);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = (sender as VisionFlow).Name;
            saveFileDialog.DefaultExt = "json";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllTextAsync(saveFileDialog.FileName, serializationString);
            }
        }
    }
}
