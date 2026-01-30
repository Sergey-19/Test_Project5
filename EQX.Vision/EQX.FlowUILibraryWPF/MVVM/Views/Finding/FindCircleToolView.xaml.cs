using EQX.Vision.Shapes;
using EQX.Vision.Tool;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EQX.FlowUILibraryWPF.MVVM.Views
{
    /// <summary>
    /// Interaction logic for FindCircleToolView.xaml
    /// </summary>
    public partial class FindCircleToolView : UserControl
    {
        public FindCircleToolView()
        {
            InitializeComponent();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender.GetType() != typeof(Thumb)) return;
            Thumb thumb = (Thumb)sender;

            if (thumb.DataContext!.GetType() != typeof(CCircle)) return;

            double deltaHorizontal = e.HorizontalChange;
            double deltaVertical = e.VerticalChange;

            (thumb.DataContext as CCircle).CenterX = (int)((Canvas.GetLeft(thumb) + deltaHorizontal) * (thumb.DataContext as CCircle).ScaleX + (thumb.DataContext as CCircle).Radius * (thumb.DataContext as CCircle).ScaleX);
            (thumb.DataContext as CCircle).CenterY = (int)((Canvas.GetTop(thumb) + deltaVertical) * (thumb.DataContext as CCircle).ScaleY + (thumb.DataContext as CCircle).Radius * (thumb.DataContext as CCircle).ScaleX);
        }

        private void ImageDisplay_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat) == null) return;

            if (this.DataContext is FindCircleTool findCircleTool)
            {
                findCircleTool.ExpectedCircle.ScaleX = (findCircleTool.Inputs["ImageMat"] as Mat).Width / (e.Source as Image).ActualWidth;
                findCircleTool.ExpectedCircle.ScaleY = (findCircleTool.Inputs["ImageMat"] as Mat).Height / (e.Source as Image).ActualHeight;
                findCircleTool.UpdateSizeExpectedCircles();
            }
        }

        private void ImageDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            if (((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat) == null) return;
            if (this.DataContext is FindCircleTool findCircleTool)
            {
                findCircleTool.ExpectedCircle.ScaleX = (findCircleTool.Inputs["ImageMat"] as Mat).Width / (e.Source as Image).ActualWidth;
                findCircleTool.ExpectedCircle.ScaleY = (findCircleTool.Inputs["ImageMat"] as Mat).Height / (e.Source as Image).ActualHeight;
            }
        }

        private void ImageDisplay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double controlWidth = (sender as Image).ActualWidth;
            double controlHeight = (sender as Image).ActualHeight;

            double imageWidth = ((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat).Width;
            double imageHeight = ((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat).Height;

            double scaleX = imageWidth / controlWidth;
            double scaleY = imageHeight / controlHeight;

            double mouseX = e.GetPosition((e.Source as Image)).X;
            double mouseY = e.GetPosition((e.Source as Image)).Y;

            double imageMouseX = mouseX * scaleX;
            double imageMouseY = mouseY * scaleY;

            if (e.Delta > 0)
            {
                (e.Source as Image).Width = controlWidth * 1.1;
                (e.Source as Image).Height = controlHeight * 1.1;
            }
            else if (e.Delta < 0)
            {
                (e.Source as Image).Width = controlWidth / 1.1;
                (e.Source as Image).Height = controlHeight / 1.1;
            }

            double newImageMouseX = imageMouseX * ((e.Source as Image).Width / imageWidth);
            double newImageMouseY = imageMouseY * ((e.Source as Image).Height / imageHeight);

            double offsetX = newImageMouseX - mouseX;
            double offsetY = newImageMouseY - mouseY;

            (((e.Source as Image).Parent as Grid).Parent as ScrollViewer).HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            (((e.Source as Image).Parent as Grid).Parent as ScrollViewer).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            (((e.Source as Image).Parent as Grid).Parent as ScrollViewer).ScrollToHorizontalOffset((((e.Source as Image).Parent as Grid).Parent as ScrollViewer).HorizontalOffset + offsetX);
            (((e.Source as Image).Parent as Grid).Parent as ScrollViewer).ScrollToVerticalOffset((((e.Source as Image).Parent as Grid).Parent as ScrollViewer).VerticalOffset + offsetY);
        }

        private void ImageDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point controlMousePos = e.GetPosition(e.Source as Image);

            double imageControlWidth = (e.Source as Image).ActualWidth;
            double imageControlHeight = (e.Source as Image).ActualHeight;

            double imageWidth = ((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat).Width;
            double imageHeight = ((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat).Height;

            double scaleX = imageWidth / imageControlWidth;
            double scaleY = imageHeight / imageControlHeight;

            int X = (int)(controlMousePos.X * scaleX);
            int Y = (int)(controlMousePos.Y * scaleY);

            lblX.Content = X;
            lblY.Content = Y;
            if (X > 0 && Y > 0)
            {
                byte value = ((this.DataContext as FindCircleTool).Inputs["ImageMat"] as Mat).At<byte>(Y, X);
                lblColorValue.Content = value;
            }
        }
    }
}
