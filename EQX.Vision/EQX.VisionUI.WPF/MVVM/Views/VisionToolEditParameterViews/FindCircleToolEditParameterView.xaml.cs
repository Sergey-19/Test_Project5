using EQX.Vision.Algorithms;
using EQX.VisionUI.WPF.Adorners;
using Microsoft.Win32;
using OpenCvSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace EQX.VisionUI.WPF.MVVM.Views.VisionToolEditParameterViews
{
    /// <summary>
    /// Interaction logic for FindCircleToolEditParameterView.xaml
    /// </summary>
    public partial class FindCircleToolEditParameterView : UserControl
    {
        public FindCircleToolEditParameterView()
        {
            InitializeComponent();
        }

        private void DrawAdorner()
        {
            AdornerCanvas.Children.Clear();
            if (!(this.DataContext is FindCircleTool findCircleTool)) return;
            if (!(findCircleTool.Inputs["ImageMat"] is Mat inputMat)) return;

            double sizeFactorX = ImageDisplay.RenderSize.Width / inputMat.Width;
            double sizeFactorY = ImageDisplay.RenderSize.Height / inputMat.Height;

            Border border = new Border()
            {
                Width = findCircleTool.ExpectedCircle.Radius * 2 * sizeFactorX,
                Height = findCircleTool.ExpectedCircle.Radius * 2 * sizeFactorY,
                BorderBrush = Brushes.Green,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(9999),
            };

            Canvas.SetLeft(border, (findCircleTool.ExpectedCircle.CenterX - findCircleTool.ExpectedCircle.Radius) * sizeFactorX);
            Canvas.SetTop(border,(findCircleTool.ExpectedCircle.CenterY - findCircleTool.ExpectedCircle.Radius) * sizeFactorX);

            AdornerCanvas.Children.Add(border);
            CircleResizeDragAdorner resizeAdorner = new CircleResizeDragAdorner(border) { Description = "Circle" };

            resizeAdorner.AdornerArranged += (obj, agr) =>
            {
                findCircleTool.ExpectedCircle.Radius = (int)((((CircleResizeDragAdorner)obj).AdornedElement.DesiredSize.Width / sizeFactorX) / 2);
                findCircleTool.ExpectedCircle.CenterX = (int)(Canvas.GetLeft(border) / sizeFactorX) + findCircleTool.ExpectedCircle.Radius;
                findCircleTool.ExpectedCircle.CenterY = (int)(Canvas.GetTop(border) / sizeFactorY) + findCircleTool.ExpectedCircle.Radius;
            };

            AdornerLayer.GetAdornerLayer(AdornerCanvas).Add(resizeAdorner);
        }
        private void ImageDisplay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(this.DataContext is FindCircleTool findCircleTool)) return;
            if (!(findCircleTool.Inputs["ImageMat"] is Mat inputMat)) return;

            double controlWidth = ImageDisplay.ActualWidth;
            double controlHeight = ImageDisplay.ActualHeight;

            double imageWidth = inputMat.Width;
            double imageHeight = inputMat.Height;

            double scaleX = imageWidth / controlWidth;
            double scaleY = imageHeight / controlHeight;

            double mouseX = e.GetPosition((e.Source as Image)).X;
            double mouseY = e.GetPosition((e.Source as Image)).Y;

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

            double newImageMouseX = imageMouseX * ((e.Source as Image).Width / imageWidth);
            double newImageMouseY = imageMouseY * ((e.Source as Image).Height / imageHeight);

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

        private void SaveImageMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is FindCircleTool findCircleTool)) return;
            if (!(findCircleTool.Inputs["ImageMat"] is Mat inputMat)) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "BMP";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] unicodeBytes = System.Text.Encoding.Unicode.GetBytes(saveFileDialog.FileName);
                string encodedString = System.Text.Encoding.Unicode.GetString(unicodeBytes);
                Cv2.ImWrite(encodedString, (Mat)findCircleTool.Inputs["ImageMat"]);
            }
        }

        private void FitImageClick(object sender, RoutedEventArgs e)
        {
            FitImage();
        }

        private void ImageDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            FitImage();
        }

        private void FitImage()
        {
            ImageDisplay.Width = ScrollImage.ActualWidth;
            ImageDisplay.Height = ScrollImage.ActualHeight;

            ScrollImage.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ScrollImage.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }
    }
}
