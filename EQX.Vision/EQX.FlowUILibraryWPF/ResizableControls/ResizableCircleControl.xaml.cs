using EQX.FlowUILibraryWPF.MVVM.Views;
using EQX.Vision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EQX.FlowUILibraryWPF.ResizableControls
{
    /// <summary>
    /// Interaction logic for ResizableCircleControl.xaml
    /// </summary>
    public partial class ResizableCircleControl : UserControl
    {
        private bool _isResizing = false;
        private Point _startPoint;
        public ResizableCircleControl()
        {
            InitializeComponent();
        }
        private void PanelPointReleased(object? sender, MouseButtonEventArgs e)
        {
            _isResizing = false;
        }

        private void PanelPointMoved(object? sender, MouseEventArgs e)
        {
            if (_isResizing)
            {
                var currentPosition = e.GetPosition(this);
                var deltaX = currentPosition.X - _startPoint.X;

                if (this.DataContext is CCircle)
                {
                    if ((this.DataContext as CCircle)!.ScaleX < 1)
                    {
                        (this.DataContext as CCircle)!.CenterX = (int)((this.DataContext as CCircle)!.CenterX + (deltaX / 2));
                        (this.DataContext as CCircle)!.Radius = (int)((this.DataContext as CCircle)!.Radius + (deltaX / 2));
                    }
                    else
                    {
                        (this.DataContext as CCircle)!.CenterX = (int)((this.DataContext as CCircle)!.CenterX + (deltaX / 2) * (this.DataContext as CCircle)!.ScaleX);
                        (this.DataContext as CCircle)!.Radius = (int)((this.DataContext as CCircle)!.Radius + (deltaX / 2) * (this.DataContext as CCircle)!.ScaleX);
                    }
                    _startPoint = currentPosition;
                }
            }
        }

        private void ResizePointPressed(object? sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ToolSettingWindowView.GetWindow(this).MouseLeftButtonUp += PanelPointReleased;
            ToolSettingWindowView.GetWindow(this).MouseMove += PanelPointMoved;
            _isResizing = true;
            _startPoint = e.GetPosition(this);
        }
    }
}
