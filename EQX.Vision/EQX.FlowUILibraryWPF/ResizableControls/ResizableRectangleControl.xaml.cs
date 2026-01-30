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
    /// Interaction logic for ResizableRectangleControl.xaml
    /// </summary>
    public partial class ResizableRectangleControl : UserControl
    {
        private bool _isRightBottomCornerPress;
        private bool _isCenterEdgeBottomPress;
        private bool _isCenterEdgeRightPress;
        private Point _startPoint;
        public ResizableRectangleControl()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(this);
            if (this.DataContext is CRectangle rect)
            {
                if (_isCenterEdgeBottomPress)
                {
                    var deltaY = currentPosition.Y - _startPoint.Y;
                    rect.Height += deltaY * rect.ScaleY;

                    _startPoint = currentPosition;
                }
                else if (_isCenterEdgeRightPress)
                {
                    var deltaX = currentPosition.X - _startPoint.X;
                    rect.Width += deltaX * rect.ScaleX;

                    _startPoint = currentPosition;
                }
                else if (_isRightBottomCornerPress)
                {
                    var deltaX = currentPosition.X - _startPoint.X;
                    var deltaY = currentPosition.Y - _startPoint.Y;
                    rect.Width += deltaX * rect.ScaleX;
                    rect.Height += deltaY * rect.ScaleY;

                    _startPoint = currentPosition;
                }
            }
            UpdatePositionPoint();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isRightBottomCornerPress = false;
            _isCenterEdgeBottomPress = false;
            _isCenterEdgeRightPress = false;
        }

        private void RightBottomCornerPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToolSettingWindowView.GetWindow(this).MouseLeftButtonUp += StackPanel_MouseLeftButtonUp;
            ToolSettingWindowView.GetWindow(this).MouseMove += StackPanel_MouseMove;

            _isRightBottomCornerPress = true;
            _startPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void CenterEdgeBottomPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToolSettingWindowView.GetWindow(this).MouseLeftButtonUp += StackPanel_MouseLeftButtonUp;
            ToolSettingWindowView.GetWindow(this).MouseMove += StackPanel_MouseMove;

            _isCenterEdgeBottomPress = true;
            _startPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void CenterEdgeRightPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToolSettingWindowView.GetWindow(this).MouseLeftButtonUp += StackPanel_MouseLeftButtonUp;
            ToolSettingWindowView.GetWindow(this).MouseMove += StackPanel_MouseMove;

            _isCenterEdgeRightPress = true;
            _startPoint = e.GetPosition(this);
            e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is CRectangle rect)
            {
                rect.ScaleChangedEvent += ScaleChangedEventHanlder;
            }
            UpdatePositionPoint();
        }

        private void ScaleChangedEventHanlder(object? sender, EventArgs e)
        {
            UpdatePositionPoint();
        }

        private void UpdatePositionPoint()
        {
            if (this.DataContext is CRectangle rectangle)
            {
                RightBottomCornerPoint.Margin = new Thickness(-5, rectangle.HeightUI, 0, 0);
                CenterEdgeBottomPoint.Margin = new Thickness(-rectangle.WidthUI, rectangle.HeightUI, 0, 0);
                CenterEdgeRightPoint.Margin = new Thickness(-10, 0, 0, 0);
            }
        }
    }
}
