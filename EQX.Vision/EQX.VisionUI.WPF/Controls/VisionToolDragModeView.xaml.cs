using System.Windows.Controls;
using System.Windows.Input;

namespace EQX.VisionUI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for VisionToolDragModeView.xaml
    /// </summary>
    public partial class VisionToolDragModeView : UserControl
    {
        public MouseButtonEventHandler? KeyInputMouseDownEvent;
        public MouseButtonEventHandler? KeyInputMouseUpEvent;

        public MouseButtonEventHandler? KeyOutputMouseDownEvent;
        public MouseButtonEventHandler? KeyOutputMouseUpEvent;

        public VisionToolDragModeView()
        {
            InitializeComponent();
        }

        private void KeyInputMouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyInputMouseDownEvent?.Invoke(this.DataContext, e);
            e.Handled = true;
        }

        private void KeyInputMouseUp(object sender, MouseButtonEventArgs e)
        {
            KeyInputMouseUpEvent?.Invoke(this.DataContext, e);
            e.Handled = true;
        }

        private void KeyOutputMouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyOutputMouseDownEvent?.Invoke(this.DataContext, e );
            e.Handled = true;
        }

        private void KeyOutputMouseUp(object sender, MouseButtonEventArgs e)
        {
            KeyOutputMouseUpEvent?.Invoke(this.DataContext, e);
            e.Handled = true;
        }
    }
}
