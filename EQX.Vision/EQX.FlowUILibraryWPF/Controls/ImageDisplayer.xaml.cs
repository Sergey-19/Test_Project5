using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace EQX.FlowUILibraryWPF.Controls
{
    /// <summary>
    /// Interaction logic for ImageDisplayer.xaml
    /// </summary>
    public partial class ImageDisplayer : UserControl , INotifyPropertyChanged
    {
        public Mat ImageDisplay
        {
            get 
            {
                return (Mat)GetValue(ImageDisplayProperty); 
            }
            set 
            {
                SetValue(ImageDisplayProperty, value); 
                OnPropertyChanged(nameof(ImageDisplay));
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageDisplayProperty =
            DependencyProperty.Register("ImageDisplay", typeof(Mat), typeof(ImageDisplayer), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
        public ImageDisplayer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point controlMousePos = e.GetPosition(e.Source as Image);

            double imageControlWidth = (e.Source as Image).ActualWidth;
            double imageControlHeight = (e.Source as Image).ActualHeight;

            double imageWidth = ImageDisplay.Width;
            double imageHeight = ImageDisplay.Height;

            double scaleX = imageWidth / imageControlWidth;
            double scaleY = imageHeight / imageControlHeight;

            int X = (int)(controlMousePos.X * scaleX);
            int Y = (int)(controlMousePos.Y * scaleY);

            lblX.Content = X;
            lblY.Content = Y;
            if (X > 0 && Y > 0)
            {
                byte value = ImageDisplay.At<byte>(Y, X);
                lblColorValue.Content = value;
            }
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double controlWidth = (sender as Image).ActualWidth;
            double controlHeight = (sender as Image).ActualHeight;

            double imageWidth = ImageDisplay.Width;
            double imageHeight = ImageDisplay.Height;

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

        private void SaveImageMenuItemClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if(saveFileDialog.ShowDialog() == true) 
            {
                Cv2.ImWrite(saveFileDialog.FileName, ImageDisplay);
            }
        }
    }
}
