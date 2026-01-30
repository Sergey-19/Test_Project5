using EQX.Vision.Algorithms;
using EQX.VisionUI.WPF.Adorners;
using OpenCvSharp;
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

namespace EQX.VisionUI.WPF.MVVM.Views.VisionToolEditParameterViews
{
    /// <summary>
    /// Interaction logic for HoughCircleToolEditParameterView.xaml
    /// </summary>
    public partial class HoughCircleToolEditParameterView : UserControl
    {
        public HoughCircleToolEditParameterView()
        {
            InitializeComponent();
        }

        private void DrawAdorner()
        {
            AdornerCanvas.Children.Clear();
            if (!(this.DataContext is HoughCircleTool houghCircleTool)) return;
            if (!(houghCircleTool.Inputs["ImageMat"] is Mat inputMat)) return;

            double sizeFactorX = ImageDisplay.RenderSize.Width / inputMat.Width;
            double sizeFactorY = ImageDisplay.RenderSize.Height / inputMat.Height;

            if (!(houghCircleTool.Parameters["ROI"] is CRectangle rect)) return;
            Border border = new Border()
            {
                Width = rect.Width * sizeFactorX,
                Height = rect.Height * sizeFactorY,
                BorderBrush = Brushes.Blue,
                BorderThickness = new Thickness(1),
            };

            Canvas.SetLeft(border, rect.X * sizeFactorX);
            Canvas.SetTop(border, rect.Y * sizeFactorY);

            AdornerCanvas.Children.Add(border);

            RectangleResizeDragAdorner resizeAdorner = new RectangleResizeDragAdorner(border) { Description = "ROI" };
            resizeAdorner.AdornerArranged += (obj, agr) =>
            {
                rect.Width = (int)(((RectangleResizeDragAdorner)obj).AdornedElement.DesiredSize.Width / sizeFactorX);
                rect.Height = (int)(((RectangleResizeDragAdorner)obj).AdornedElement.DesiredSize.Height / sizeFactorY);
                rect.X = (int)(Canvas.GetLeft(((RectangleResizeDragAdorner)obj).AdornedElement) / sizeFactorX);
                rect.Y = (int)(Canvas.GetTop(((RectangleResizeDragAdorner)obj).AdornedElement) / sizeFactorY);
            };

            if (AdornerLayer.GetAdornerLayer(AdornerCanvas) == null) return;
            AdornerLayer.GetAdornerLayer(AdornerCanvas).Add(resizeAdorner);
        }
        private void ImageDisplay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(this.DataContext is HoughCircleTool houghCircleTool)) return;
            if (!(houghCircleTool.Inputs["ImageMat"] is Mat inputMat)) return;
            if (!(e.Source is Image image)) return;

            double controlWidth = ImageDisplay.ActualWidth;
            double controlHeight = ImageDisplay.ActualHeight;

            double imageWidth = inputMat.Width;
            double imageHeight = inputMat.Height;

            double scaleX = imageWidth / controlWidth;
            double scaleY = imageHeight / controlHeight;

            double mouseX = e.GetPosition(image).X;
            double mouseY = e.GetPosition(image).Y;

            double imageMouseX = mouseX * scaleX;
            double imageMouseY = mouseY * scaleY;

            if (e.Delta > 0)
            {
                (ImageDisplay).Width = controlWidth * 1.1;
                (ImageDisplay).Height = controlHeight * 1.1;
            }
            else if (e.Delta < 0)
            {
                (ImageDisplay).Width = controlWidth / 1.1;
                (ImageDisplay).Height = controlHeight / 1.1;
            }

            double newImageMouseX = imageMouseX * (image.Width / imageWidth);
            double newImageMouseY = imageMouseY * (image.Height / imageHeight);

            double offsetX = newImageMouseX - mouseX;
            double offsetY = newImageMouseY - mouseY;

            (ScrollImage).HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            (ScrollImage).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            (ScrollImage).ScrollToHorizontalOffset(ScrollImage.HorizontalOffset + offsetX);
            (ScrollImage).ScrollToVerticalOffset(ScrollImage.VerticalOffset + offsetY);

        }
        private void ImageDisplay_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAdorner();
        }
    }
}
