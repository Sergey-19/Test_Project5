using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms;
using EQX.VisionUI.WPF.Comparer;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EQX.VisionUI.WPF.MVVM.ViewModels
{
    public class VisionTeachingViewModel : ObservableObject
    {
        #region Events
        public EventHandler LoadVisionFlowHandler;
        #endregion

        #region Properties
        public List<IVisionTool> VisionTools { get; }
        public ObservableCollection<IVisionFlow> VisionFlows { get; }
        public VisionFlow CurrentFlow
        {
            get => currentFlow;
            set
            {
                currentFlow = value;
                OnPropertyChanged();
            }
        }
        public IVisionTool SelectedVisionTool
        {
            get 
            {
                return _selectedVisionTool; 
            }
            set 
            {
                _selectedVisionTool = value;
                OnPropertyChanged(nameof(SelectedVisionTool));
            }
        }
        #endregion

        #region Constructors
        public VisionTeachingViewModel(IVisionToolRepository visionToolRepository,
            IVisionFlowRepository visionFlowRepository)
        {
            VisionTools = visionToolRepository.GetAll().ToList();
            VisionFlows = new ObservableCollection<IVisionFlow>(visionFlowRepository.GetAll().ToList());

            if (VisionFlows.Count > 0)
            {
                CurrentFlow = (VisionFlow)VisionFlows.First();
            }
        }
        #endregion

        #region Public methods
        public void AddToolToCurrentFlow(IVisionTool visionTool, EQX.Vision.Algorithms.Point position)
        {
            if (CurrentFlow.VisionTools.Contains(visionTool)) return;

            CurrentFlow.VisionTools.Add(visionTool);
            CurrentFlow.FlowDescription.VisionToolDescriptions.Add(new VisionToolDescription(visionTool, position));
        }

        public void RemoveToolFromCurrentFlow(IVisionTool visionTool)
        {
            CurrentFlow.VisionToolConnections.Where(vtc => vtc.OriginToolId == visionTool.Id || vtc.TargetToolId == visionTool.Id).ToList().ForEach(vtc =>
            {
                CurrentFlow.VisionToolConnections.Remove(vtc);
            });

            CurrentFlow.VisionTools.Remove(visionTool);
            CurrentFlow.FlowDescription.VisionToolDescriptions.RemoveAll(vtd => vtd.Id == visionTool.Id);
        }
        public void RemovePathConnectionFromCurrentFlow(Geometry geometryConnection)
        {
            int index = CurrentFlow.FlowDescription.PathGeometries.ToList().FindIndex(g => new GeometryComparer().Equals(g, geometryConnection));
            CurrentFlow.VisionToolConnections.RemoveAt(index);

        }

        public void UpdateVisionToolLocation(IVisionTool visionTool, EQX.Vision.Algorithms.Point newPosition)
        {
            var visionToolDescription = CurrentFlow.FlowDescription.VisionToolDescriptions.FirstOrDefault(vtd => vtd.Id == visionTool.Id);
            if (visionToolDescription == null) return;

            visionToolDescription.Position.X = newPosition.X;
            visionToolDescription.Position.Y = newPosition.Y;

            CurrentFlow.FlowDescription.VisiontoolConnectionsChanged(null,null);
        }

        public EQX.Vision.Algorithms.Point GetOriginPoint(IVisionTool visionTool, int index)
        {
            var visionToolDescription = CurrentFlow.FlowDescription.VisionToolDescriptions.FirstOrDefault(vtd => vtd.Id == visionTool.Id);
            if (visionToolDescription == null) return new EQX.Vision.Algorithms.Point(0,0);

            return visionToolDescription.GetOriginPoint(index);
        }

        public EQX.Vision.Algorithms.Point GetTargetPoint(IVisionTool visionTool, int index)
        {
            var visionToolDescription = CurrentFlow.FlowDescription.VisionToolDescriptions.FirstOrDefault(vtd => vtd.Id == visionTool.Id);
            if (visionToolDescription == null) return new EQX.Vision.Algorithms.Point(0, 0);

            return visionToolDescription.GetTargetPoint(index);
        }
        #endregion

        #region Commands
        public ICommand RunFlowCommand
        {
            get
            {
                return new AsyncRelayCommand(async () =>
                {
                    var stopWatch = new Stopwatch();
                    await CurrentFlow.RunAsync(5000);
                });
            }
        }
        public ICommand SaveFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CurrentFlow.Save();
                });
            }
        }
        public ICommand LoadFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LoadVisionFlowHandler?.Invoke(this,EventArgs.Empty);
                });
            }
        }
        #endregion

        #region Privates
        private VisionFlow currentFlow;
        private IVisionTool _selectedVisionTool;
        #endregion
    }
}
