using EQX.Vision.Algorithms;
using EQX.Vision.Grabber.Helpers;
using Microsoft.Win32;
using OpenCvSharp;
using System.Windows;
using System.Windows.Controls;

namespace EQX.VisionUI.WPF.MVVM.Views.VisionToolEditParameterViews
{
    /// <summary>
    /// Interaction logic for GrabToolEditParameterView.xaml
    /// </summary>
    public partial class GrabToolEditParameterView : UserControl
    {
        public GrabToolEditParameterView()
        {
            InitializeComponent();
            InitComboBox();
        }

        private void InitComboBox()
        {
            var values1 = Enum.GetValues(typeof(ImreadModes));
            cbReadModes.ItemsSource = values1;
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is GrabTool grabTool)) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                grabTool.Parameters["ImagePath"] = openFileDialog.FileName;
            }
        }

        private void GrabButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is GrabTool grabTool)) return;
            if(grabTool.Camera == null) return;

            grabTool.Outputs["ImageMat"] =  grabTool.Camera.GrabSingle().ToMat();
        }

        private void LiveOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is GrabTool grabTool)) return;
            if (grabTool.Camera == null) return;

            grabTool.Camera.ContinuousImageGrabStart();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is GrabTool grabTool)) return;
            if (grabTool.Camera == null) return;

            grabTool.Camera.ContinuousImageGrabStop();
        }
    }
}
