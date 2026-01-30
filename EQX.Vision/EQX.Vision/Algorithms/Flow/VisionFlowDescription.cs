using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms.Model.Parameters;
using EQX.Vision.Algorithms.Model.ToolResult;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;

namespace EQX.Vision.Algorithms
{
    public class VisionFlowDescription : ObservableObject
    {
        public VisionFlowDescription(IVisionFlow visionFlow)
        {
            _visionFlow = visionFlow;

            VisionToolDescriptions = new List<VisionToolDescription>();
        }

        public VisionFlowDescription(IVisionFlow visionFlow, List<VisionToolDescription> visionToolDescriptions)
        {
            _visionFlow = visionFlow;
            VisionToolDescriptions = visionToolDescriptions;

            if (_visionFlow.VisionTools.FirstOrDefault(vt => vt is OutputTool) != null)
            {
                IVisionTool outputTool = _visionFlow.VisionTools.FirstOrDefault(vt => vt is OutputTool);

                outputTool.Inputs.Keys.CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        if (_visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == ((KeyType)e.OldItems[e.OldItems.Count - 1]).Key) != null)
                        {
                            var connection = _visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == ((KeyType)e.OldItems[e.OldItems.Count - 1]).Key);
                            _visionFlow.VisionToolConnections.Remove(connection);
                        }
                    }
                };
            }
        }
        [JsonConstructor]
        public VisionFlowDescription(List<VisionToolDescription> visionToolDescriptions)
        {
            VisionToolDescriptions = visionToolDescriptions;
        }
        public List<VisionToolDescription> VisionToolDescriptions { get; }

        [JsonIgnore]
        public ObservableCollection<Geometry> PathGeometries
        {
            get
            {
                ObservableCollection<Geometry> result = new ObservableCollection<Geometry>();

                foreach (var connection in _visionFlow.VisionToolConnections)
                {
                    VisionToolDescription originDescription = VisionToolDescriptions.First(vtd => vtd.Id == connection.OriginToolId);
                    VisionToolDescription targetDescription = VisionToolDescriptions.First(vtd => vtd.Id == connection.TargetToolId);

                    IVisionTool originVisionTool = _visionFlow.VisionTools.First(vt => vt.Id == connection.OriginToolId);
                    IVisionTool targetVisionTool = _visionFlow.VisionTools.First(vt => vt.Id == connection.TargetToolId);

                    int originKeyIndex = originVisionTool.Outputs.Keys.IndexOf(connection.OriginKey) + 1;
                    int targetKeyIndex = targetVisionTool.Inputs.Keys.IndexOf(connection.TargetKey) + 1;

                    Point originPoint = originDescription.GetOriginPoint(originKeyIndex);
                    Point targetPoint = targetDescription.GetTargetPoint(targetKeyIndex);
                    result.Add(VisionFlowHelpers.CreateGeometryPath(originPoint, targetPoint, 1));
                }
                // visionFlow.VisionToolConnections
                // VisionToolDescriptions
                return result;
            }
        }
        protected readonly IVisionFlow _visionFlow;
        public void VisiontoolConnectionsChanged(ObservableCollection<VisionToolConnection> visionToolConnection, EventArgs e)
        {
            if (visionToolConnection == null)
            {
                OnPropertyChanged(nameof(PathGeometries));
                return;
            }

            OnPropertyChanged(nameof(PathGeometries));

            if ((e as NotifyCollectionChangedEventArgs).Action != NotifyCollectionChangedAction.Add) return;

            if ((e as NotifyCollectionChangedEventArgs).NewItems[0] is VisionToolConnection visionToolConnectionAdded == false) return;

            if (_visionFlow.VisionTools.FirstOrDefault(vt => vt is OutputTool) != null)
            {
                IVisionTool outputTool = _visionFlow.VisionTools.FirstOrDefault(vt => vt is OutputTool);

                outputTool.Inputs.Keys.CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        if (_visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == ((KeyType)e.OldItems[e.OldItems.Count - 1]).Key) != null)
                        {
                            var connection = _visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == ((KeyType)e.OldItems[e.OldItems.Count - 1]).Key);
                            _visionFlow.VisionToolConnections.Remove(connection);
                        }
                    }
                    ((OutputTool)outputTool).UpdateParameters();
                };
                if (visionToolConnection.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id) == null) return;

                string originKey = visionToolConnectionAdded.OriginKey;
                string targetKey = visionToolConnectionAdded.TargetKey;

                if (outputTool.Inputs.Keys.Contains(targetKey) == false) return;

                if (outputTool.Inputs.Keys.Contains(originKey) &&
                    (_visionFlow.VisionToolConnections.Where(vtc => vtc.TargetToolId == outputTool.Id && vtc.OriginKey == originKey).Count() > 1))
                {
                    outputTool.Inputs.Keys[outputTool.Inputs.Keys.IndexOf(originKey)].Key = $"Output {outputTool.Inputs.Keys.IndexOf(originKey)}";
                    if (_visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == originKey && vtc.OriginKey == originKey) != null)
                    {
                        var connection = _visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == originKey && vtc.OriginKey == originKey);
                        _visionFlow.VisionToolConnections.Remove(connection);
                    }
                    outputTool.Inputs.Keys[outputTool.Inputs.Keys.IndexOf(targetKey)].Key = originKey;
                    _visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == targetKey).TargetKey = originKey;

                    return;
                }
                outputTool.Inputs.Keys[outputTool.Inputs.Keys.IndexOf(targetKey)].Key = originKey;
                outputTool.Inputs.Keys[outputTool.Inputs.Keys.IndexOf(originKey)].Type 
                    = _visionFlow.VisionTools.First(vt => vt.Id == visionToolConnectionAdded.OriginToolId).Outputs.Keys.First(kt => kt.Key == originKey).Type;

                _visionFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == outputTool.Id && vtc.TargetKey == targetKey).TargetKey = originKey;

                ((OutputTool)outputTool).UpdateParameters();
            }
        }
    }
}
