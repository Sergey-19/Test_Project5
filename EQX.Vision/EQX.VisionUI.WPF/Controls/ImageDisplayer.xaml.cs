using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EQX.VisionUI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ImageDisplayer.xaml
    /// </summary>
    public partial class ImageDisplayer : UserControl, INotifyPropertyChanged
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
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageDisplayProperty =
            DependencyProperty.Register("ImageDisplay", typeof(Mat), typeof(ImageDisplayer), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ImageDisplayer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ImageDisplay == null) return;
            if ((sender is Image image) == false) return;

            double controlWidth = image.ActualWidth;
            double controlHeight = image.ActualHeight;

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
                image.Width = controlWidth * 1.1;
                image.Height = controlHeight * 1.1;
            }
            else if (e.Delta < 0)
            {
                image.Width = controlWidth / 1.1;
                image.Height = controlHeight / 1.1;
            }

            double newImageMouseX = imageMouseX * ((e.Source as Image).Width / imageWidth);
            double newImageMouseY = imageMouseY * ((e.Source as Image).Height / imageHeight);

            double offsetX = newImageMouseX - mouseX;
            double offsetY = newImageMouseY - mouseY;

            scrollImage.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollImage.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            scrollImage.ScrollToHorizontalOffset(scrollImage.HorizontalOffset + offsetX);
            scrollImage.ScrollToVerticalOffset(scrollImage.VerticalOffset + offsetY);
        }

        private void SaveImageMenuItemClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "BMP";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                byte[] unicodeBytes = System.Text.Encoding.Unicode.GetBytes(saveFileDialog.FileName);
                string encodedString = System.Text.Encoding.Unicode.GetString(unicodeBytes);
                Cv2.ImWrite(encodedString, ImageDisplay);
            }
        }

        private bool _isShowCenterLines;

        public bool IsShowCenterLines
        {
            get { return _isShowCenterLines; }
            set { _isShowCenterLines = value; OnPropertyChanged(nameof(IsShowCenterLines)); }
        }
        private void FitImageClick(object sender, RoutedEventArgs e)
        {
            FitImage();
        }

        private void FitImage()
        {
            imageViewer.Width = scrollImage.ActualWidth;
            imageViewer.Height = scrollImage.ActualHeight;

            scrollImage.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollImage.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private void imageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            FitImage();
        }
    }
}
