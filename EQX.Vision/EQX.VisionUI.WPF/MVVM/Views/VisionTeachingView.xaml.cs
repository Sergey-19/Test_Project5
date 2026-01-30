using EQX.VisionUI.WPF.Controls;
using EQX.VisionUI.WPF.MVVM.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EQX.FlowUILibraryWPF.Algorithms.Models;
using EQX.FlowUILibraryWPF.Algorithms.Defines;
using EQX.Core.Vision.Algorithms;
using EQX.Vision.Algorithms;

namespace EQX.VisionUI.WPF.MVVM.Views
{
    /// <summary>
    /// Interaction logic for VisionTeachingView.xaml
    /// </summary>
    public partial class VisionTeachingView : UserControl
    {
        public VisionTeachingView()
        {
            InitializeComponent();
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                VisionToolDragModeView toolView = new VisionToolDragModeView();

                toolView.DataContext = Activator.CreateInstance(((FrameworkElement)sender).DataContext.GetType());

                DragDrop.DoDragDrop(toolView, new DataObject(DataFormats.Serializable, toolView), DragDropEffects.Move);
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is FrameworkElement element)
            {
                element.Opacity = 0.5;
                System.Windows.Point dropPosition = e.GetPosition(canvas);

                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);

                if (!canvas.Children.Contains(element))
                {
                    canvas.Children.Add(element);
                }
            }
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is FrameworkElement element)
            {
                System.Windows.Point dropPosition = e.GetPosition(canvas);

                dropPosition.X = 25 * Math.Round(dropPosition.X / 25, 0);
                dropPosition.Y = 25 * Math.Round(dropPosition.Y / 25, 0);

                canvas.Children.Remove(element);

                ((VisionTeachingViewModel)this.DataContext).AddToolToCurrentFlow((IVisionTool)element.DataContext, new EQX.Vision.Algorithms.Point(dropPosition.X, dropPosition.Y));
                var tool = (IVisionTool)element.DataContext;
                foreach (var item in tool.Inputs.Keys)
                {
                    foreach (var item1 in tool.Outputs.Keys)
                    {
                        Port p1 = new Port(tool.Id, item.Key, PortType.Input);
                        Port p2 = new Port(tool.Id, item1.Key, PortType.Output);
                        if (Ports.Contains(p1) == false) Ports.Add(p1);
                        if (Ports.Contains(p2) == false) Ports.Add(p2);
                        int i1 = GetIndex(p1);
                        int i2 = GetIndex(p2);
                        Graph.AddEdge(Ports[i1], Ports[i2]);
                    }
                }
            }
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            FrameworkElement thumb = (FrameworkElement)e.Source;

            double newLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            double newTop = Canvas.GetTop(thumb) + e.VerticalChange;

            if (newLeft < 0)
                newLeft = 0;
            else if (newLeft + thumb.ActualWidth > canvas.ActualWidth)
                newLeft = canvas.ActualWidth - thumb.ActualWidth;

            if (newTop < 0)
                newTop = 0;
            else if (newTop + thumb.ActualHeight > canvas.ActualHeight)
                newTop = canvas.ActualHeight - thumb.ActualHeight;

            Canvas.SetLeft(thumb, newLeft);
            Canvas.SetTop(thumb, newTop);

            ((VisionTeachingViewModel)this.DataContext).UpdateVisionToolLocation((IVisionTool)thumb.DataContext, new EQX.Vision.Algorithms.Point(newLeft, newTop));
        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            FrameworkElement thumb = (FrameworkElement)e.Source;

            RoundTopLeftPosition(thumb);

            ((VisionTeachingViewModel)this.DataContext).UpdateVisionToolLocation((IVisionTool)thumb.DataContext, new EQX.Vision.Algorithms.Point(Canvas.GetLeft(thumb), Canvas.GetTop(thumb)));
        }

        private void RoundTopLeftPosition(FrameworkElement thumb)
        {
            double newLeft = Canvas.GetLeft(thumb);
            double newTop = Canvas.GetTop(thumb);

            newLeft = 25 * Math.Round(newLeft / 25, 0);
            newTop = 25 * Math.Round(newTop / 25, 0);

            Canvas.SetLeft(thumb, newLeft);
            Canvas.SetTop(thumb, newTop);
        }

        private void KeyInputMouseDownEventHanlder(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == null) return;
            if ((e.Source is FrameworkElement) == false) return;
            if (sender == null) return;
            if ((sender is IVisionTool) == false) return;

            _isConnecting = true;

            _targetKey = ((FrameworkElement)e.Source).Tag.ToString()!;
            _targetToolId = ((IVisionTool)sender).Id;
            int keyInputIndex = ((IVisionTool)sender).Inputs.Keys!.ToList().FindIndex(kt => kt.Key == _targetKey) + 1;
            _endPoint = ((VisionTeachingViewModel)this.DataContext).GetTargetPoint((IVisionTool)sender, keyInputIndex);
        }
        private void KeyInputMouseUpEventHanlder(object sender, MouseButtonEventArgs e)
        {
            if (_isConnecting == false) return;

            if (e.Source == null) return;
            if ((e.Source is FrameworkElement) == false) return;

            if (string.IsNullOrEmpty(_targetKey) == false)
            {
                tempPath.Data = null;
                _targetKey = string.Empty;
                _isConnecting = false;
                return;
            }

            _targetKey = ((Ellipse)e.Source).Tag.ToString()!;
            _targetToolId = ((IVisionTool)sender).Id;

            if (((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == _targetToolId & vtc.TargetKey == _targetKey) != null)
            {
                var connection = ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == _targetToolId & vtc.TargetKey == _targetKey);
                ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Remove(connection);
            }
            if ((IVisionTool)sender is OutputTool == false)
            {
                if (ObjectCollectionKeyTypes.KeyTypes.ContainsKey(_targetKey) && ObjectCollectionKeyTypes.KeyTypes.ContainsKey(_originKey))
                {
                    if (ObjectCollectionKeyTypes.KeyTypes[_targetKey].FullName == ObjectCollectionKeyTypes.KeyTypes[_originKey].FullName && _targetToolId != _originToolId)
                        ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Add(new VisionToolConnection(_originToolId, _targetToolId, _originKey, _targetKey));
                    AddGraph();
                }
            }
            else
            {
                ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Add(new VisionToolConnection(_originToolId, _targetToolId, _originKey, _targetKey));
            }

            tempPath.Data = null;
            _originKey = string.Empty;
            _targetKey = string.Empty;
            _isConnecting = false;

        }
        private void KeyOutputMouseDownEventHanlder(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == null) return;
            if ((e.Source is FrameworkElement) == false) return;
            if (sender == null) return;
            if ((sender is IVisionTool) == false) return;

            _isConnecting = true;

            _originKey = ((FrameworkElement)e.Source).Tag.ToString()!;
            _originToolId = ((IVisionTool)sender).Id;
            int keyOutputIndex = ((IVisionTool)sender).Outputs.Keys!.ToList().FindIndex(kt => kt.Key == _originKey) + 1;
            _startPoint = ((VisionTeachingViewModel)this.DataContext).GetOriginPoint((IVisionTool)sender, keyOutputIndex);
        }

        private void KeyOutputMouseUpEventHanlder(object sender, MouseButtonEventArgs e)
        {
            if (_isConnecting == false) return;

            if (e.Source == null) return;
            if ((e.Source is FrameworkElement) == false) return;

            if (string.IsNullOrEmpty(_originKey) == false)
            {
                tempPath.Data = null;
                _originKey = string.Empty;
                _isConnecting = false;
                return;
            }

            _originKey = ((FrameworkElement)e.Source).Tag.ToString()!;
            _originToolId = ((IVisionTool)sender).Id;

            if (((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionTools.First(vt => vt.Id == _targetToolId) is OutputTool == false)
            {
                if (((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == _targetToolId & vtc.TargetKey == _targetKey) != null ||
                    ObjectCollectionKeyTypes.KeyTypes[_targetKey] != ObjectCollectionKeyTypes.KeyTypes[_originKey])
                {
                    var connection = ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == _targetToolId & vtc.TargetKey == _targetKey);
                    ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Remove(connection);
                }
            }
            if (((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionTools.First(vt => vt.Id == _targetToolId) is OutputTool == false)
            {
                if (ObjectCollectionKeyTypes.KeyTypes[_targetKey].FullName == ObjectCollectionKeyTypes.KeyTypes[_originKey].FullName && _originToolId != _targetToolId)
                    ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Add(new VisionToolConnection(_originToolId, _targetToolId, _originKey, _targetKey));
                AddGraph();
            }
            else
            {
                ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Add(new VisionToolConnection(_originToolId, _targetToolId, _originKey, _targetKey));
            }
            tempPath.Data = null;
            _originKey = string.Empty;
            _targetKey = string.Empty;
            _isConnecting = false;
        }

        private void VisionToolDragModeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is VisionToolDragModeView toolView)
            {
                toolView.KeyInputMouseDownEvent += KeyInputMouseDownEventHanlder;
                toolView.KeyInputMouseUpEvent += KeyInputMouseUpEventHanlder;

                toolView.KeyOutputMouseDownEvent += KeyOutputMouseDownEventHanlder;
                toolView.KeyOutputMouseUpEvent += KeyOutputMouseUpEventHanlder;

                toolView.MouseRightButtonDown += ToolView_MouseRightButtonDown;

            }
        }

        private void ToolView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is VisionToolDragModeView toolView))
                return;
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItemDelete = new MenuItem()
            {
                DataContext = toolView,
                Header = "Delete"
            };
            menuItemDelete.Click += MenuItemDelete_Click;
            contextMenu.Items.Add(menuItemDelete);
            contextMenu.IsOpen = true;
            e.Handled = true;
            toolView.ContextMenu = contextMenu;
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
                return;
            ((VisionTeachingViewModel)this.DataContext).RemoveToolFromCurrentFlow((IVisionTool)((FrameworkElement)menuItem.DataContext).DataContext);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isConnecting == false) return;

            if (string.IsNullOrEmpty(_targetKey))
            {
                System.Windows.Point currentPoint = e.GetPosition(canvas);
                _endPoint = new EQX.Vision.Algorithms.Point(currentPoint.X, currentPoint.Y);
                tempPath.Data = VisionFlowHelpers.CreateGeometryPath(_startPoint, _endPoint, 1);
            }
            else if (string.IsNullOrEmpty(_originKey))
            {
                System.Windows.Point currentPoint = e.GetPosition(canvas);
                _startPoint = new EQX.Vision.Algorithms.Point(currentPoint.X, currentPoint.Y);
                tempPath.Data = VisionFlowHelpers.CreateGeometryPath(_startPoint, _endPoint, 1);
            }
        }
        private void canvas_DragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            HitTestResult result = VisualTreeHelper.HitTest(canvas, e.GetPosition(canvas));

            if ((data is FrameworkElement element) == false) return;

            if (result != null)
            {
                canvas.Children.Remove(element);
            }
        }
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isConnecting)
            {
                _isConnecting = false;
                tempPath.Data = null;
                _originKey = string.Empty;
                _targetKey = string.Empty;
            }
            e.Handled = true;
        }

        private void VisionToolDragModeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == null) return;
            if ((e.Source is FrameworkElement) == false) return;

            ((VisionTeachingViewModel)this.DataContext).SelectedVisionTool = (IVisionTool)((FrameworkElement)sender).DataContext;
        }

        #region Privates
        private bool _isConnecting = false;

        private Vision.Algorithms.Point _startPoint;
        private Vision.Algorithms.Point _endPoint;

        private int _originToolId = 0;
        private int _targetToolId = 0;

        private string _originKey = string.Empty;
        private string _targetKey = string.Empty;


        #endregion
        private void PathMenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
                return;
            ((VisionTeachingViewModel)this.DataContext).RemovePathConnectionFromCurrentFlow((Geometry)((FrameworkElement)menuItem.DataContext).DataContext);
        }

        private void Path_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Path pathView))
                return;
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItemDelete = new MenuItem()
            {
                DataContext = pathView,
                Header = "Delete"
            };
            menuItemDelete.Click += PathMenuItemDelete_Click;
            contextMenu.Items.Add(menuItemDelete);
            contextMenu.IsOpen = true;
            e.Handled = true;
            pathView.ContextMenu = contextMenu;
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
        private void AddGraph()
        {
            if (this.DataContext is VisionTeachingViewModel dataContext == false) return;
            Port p1 = new Port(_originToolId, _originKey, PortType.Output);
            Port p2 = new Port(_targetToolId, _targetKey, PortType.Input);
            int i1 = GetIndex(p1);
            int i2 = GetIndex(p2);
            var existPaths = dataContext.CurrentFlow.VisionToolConnections.Where(p => p.TargetToolId == _targetToolId && p.TargetKey == _targetKey).ToList();
            if (existPaths != null)
            {
                foreach (var existPath in existPaths)
                {
                    dataContext.CurrentFlow.VisionToolConnections.Remove(existPath);
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
                var connection = ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.FirstOrDefault(vtc => vtc.TargetToolId == _targetToolId & vtc.TargetKey == _targetKey);
                ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Remove(connection);
                _isConnecting = false;
                return;
            }

            ((VisionTeachingViewModel)this.DataContext).CurrentFlow.VisionToolConnections.Add(new VisionToolConnection(_originToolId, _targetToolId, _originKey, _targetKey));
            _isConnecting = false;
        }
    }
}

