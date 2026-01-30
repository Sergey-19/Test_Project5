using EQX.Vision.Algorithms;
using EQX.Vision.Grabber;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EQX.Vision.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel mainVM == false) return;
            foreach(var flow in mainVM.VisionTeachingVM.VisionFlows)
            {
                if (flow.VisionTools.Any(vt => vt is GrabTool) == false) continue;

                ((GrabTool)flow.VisionTools.First(vt => vt is GrabTool)).Camera = new SimulationCamera("D:\\UTGAutoLoadUnload\\Vision\\DataSimulation");
            }
        }
    }
}
