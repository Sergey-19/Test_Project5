using System.Windows;

namespace EQX.UI.Controls
{
    /// <summary>
    /// Interaction logic for MessageBoxEx.xaml
    /// </summary>
    public partial class MessageBoxEx : Window
    {
        public MessageBoxEx()
        {
            InitializeComponent();
            Topmost = true;
        }

        public static void Show(string message, bool confirmRequest = true, string caption = "Confirm")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().First().Close();

                isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().First().Close();

                MessageBoxEx messageBoxEx = new MessageBoxEx();
                ((MessageBoxExViewModel)messageBoxEx.DataContext).ConfirmRequest = confirmRequest;
                ((MessageBoxExViewModel)messageBoxEx.DataContext).Show(message, caption);
                messageBoxEx.Show();
            });
        }

        public static bool? ShowDialog(string message, string caption = "Confirm")
        {
            return ShowDialog(message, true, caption);
        }

        public static bool? ShowDialog(string message, bool confirmRequest, string caption = "Confirm")
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().First().Close();

                isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().First().Close();

                MessageBoxEx messageBoxEx = new MessageBoxEx();
                ((MessageBoxExViewModel)messageBoxEx.DataContext).ConfirmRequest = confirmRequest;
                ((MessageBoxExViewModel)messageBoxEx.DataContext).ShowDialog(message, caption);
                messageBoxEx.ShowDialog();
                return messageBoxEx.DialogResult;
            });
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                DialogResult = true;
            }
            else
            {
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                DialogResult = false;
            }
            else
            {
                Close();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
