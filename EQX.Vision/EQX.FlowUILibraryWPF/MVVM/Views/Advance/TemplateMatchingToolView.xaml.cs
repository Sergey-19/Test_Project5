using EQX.Vision.Shapes;
using EQX.Vision.Tool;
using Microsoft.Win32;
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
    /// Interaction logic for TemplateMatchingToolView.xaml
    /// </summary>
    public partial class TemplateMatchingToolView : UserControl
    {
        public TemplateMatchingToolView()
        {
            InitializeComponent();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender.GetType() != typeof(Thumb)) return;
            Thumb thumb = (Thumb)sender;

            if (thumb.DataContext is CRectangle rect)
            {
                double deltaHorizontal = e.HorizontalChange;
                double deltaVertical = e.VerticalChange;

                rect.X = (int)((Canvas.GetLeft(thumb) + deltaHorizontal) * rect.ScaleX);
                rect.Y = (int)((Canvas.GetTop(thumb) + deltaVertical) * rect.ScaleY);
            }
        }

        private void ImageDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is TemplateMatchingTool templateMatchingTool)
            {
                if ((templateMatchingTool.Inputs["ImageMat"] as Mat) == null) return;

                templateMatchingTool.RectangleTemplate.ScaleX = (templateMatchingTool.Inputs["ImageMat"] as Mat).Width / (e.Source as Image).ActualWidth;
                templateMatchingTool.RectangleTemplate.ScaleY = (templateMatchingTool.Inputs["ImageMat"] as Mat).Height / (e.Source as Image).ActualHeight;
            }
        }

        private void ImageDisplay_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.DataContext is TemplateMatchingTool templateMatchingTool)
            {
                if ((templateMatchingTool.Inputs["ImageMat"] as Mat) == null) return;

                templateMatchingTool.RectangleTemplate.ScaleX = (templateMatchingTool.Inputs["ImageMat"] as Mat).Width / (e.Source as Image).ActualWidth;
                templateMatchingTool.RectangleTemplate.ScaleY = (templateMatchingTool.Inputs["ImageMat"] as Mat).Height / (e.Source as Image).ActualHeight;
            }
        }

        private void ImageDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.DataContext is TemplateMatchingTool templateMatchingTool)
            {
                var controlMousePosition = e.GetPosition(e.Source as Image);
                double imageControlWidth = (e.Source as Image).ActualWidth;
                double imageControlHeight = (e.Source as Image).ActualHeight;

                double imageWidth = (templateMatchingTool.Inputs["ImageMat"] as Mat).Width;
                double imageHeight = (templateMatchingTool.Inputs["ImageMat"] as Mat).Height;

                double scaleX = imageWidth / imageControlWidth;
                double scaleY = imageHeight / imageControlHeight;

                int X = (int)(controlMousePosition.X * scaleX);
                int Y = (int)(controlMousePosition.Y * scaleY);

                lblX.Content = X;
                lblY.Content = Y;
                if (X > 0 && Y > 0)
                {
                    byte value = (templateMatchingTool.Inputs["ImageMat"] as Mat).At<byte>(Y, X);
                    lblColorValue.Content = value;
                }
            }
        }

        private void ImageDisplay_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.DataContext is TemplateMatchingTool templateMatchingTool)
            {
                double controlWidth = (e.Source as Image).ActualWidth;
                double controlHeight = (e.Source as Image).ActualHeight;

                double imageWidth = (templateMatchingTool.Inputs["ImageMat"] as Mat).Width;
                double imageHeight = (templateMatchingTool.Inputs["ImageMat"] as Mat).Height;

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
                var scrollView = (((e.Source as Image).Parent as Grid).Parent as ScrollViewer);
                scrollView.ScrollToHorizontalOffset(scrollView.HorizontalOffset + offsetX);
                scrollView.ScrollToVerticalOffset(scrollView.VerticalOffset + offsetY);
                e.Handled = true;
            }
        }

        private void SetTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is TemplateMatchingTool templateMatchingTool)
            {
                if ((templateMatchingTool.Inputs["ImageMat"] as Mat) == null) return;

                OpenCvSharp.Rect rect = new OpenCvSharp.Rect((int)templateMatchingTool.RectangleTemplate.X,
                    (int)templateMatchingTool.RectangleTemplate.Y,
                    (int)templateMatchingTool.RectangleTemplate.Width,
                    (int)templateMatchingTool.RectangleTemplate.Height);

                templateMatchingTool.TemplateMat = (templateMatchingTool.Inputs["ImageMat"] as Mat).SubMat(rect);
            }
            
        }

        private void LoadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                (this.DataContext as TemplateMatchingTool).TemplateMat = new Mat(openFileDialog.FileName, ImreadModes.Grayscale);
            }
        }
        private void SaveTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as TemplateMatchingTool).TemplateMat == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                Cv2.ImWrite(saveFileDialog.FileName, (this.DataContext as TemplateMatchingTool).TemplateMat);
            }
        }
    }
}
