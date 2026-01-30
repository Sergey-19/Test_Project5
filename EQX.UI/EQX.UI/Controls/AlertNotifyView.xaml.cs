using EQX.Core.Common;
using EQX.Core.Helpers;
using System.Windows;
using System.Windows.Input;

namespace EQX.UI.Controls
{
    /// <summary>
    /// Interaction logic for AlertNotifyView.xaml
    /// </summary>
    public partial class AlertNotifyView : Window
    {
        public AlertModel AlertModel { get; set; }
        public bool IsWarning { get; set; }

        public AlertNotifyView()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public static bool? ShowDialog(AlertModel alarmModel, bool isWarning = false)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().First().Close();

                isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().First().Close();


                var mainWindow = Application.Current.MainWindow;
                double width = mainWindow.ActualWidth;
                double height = mainWindow.ActualHeight;

                AlertNotifyView messageBoxEx = new AlertNotifyView()
                {
                    IsWarning = isWarning,
                    AlertModel = alarmModel,
                    Width = width * 0.6,
                    Height = height * 0.6,
                };

                messageBoxEx.ShowDialog();
                return messageBoxEx.DialogResult;
            });
        }

        public static bool? ShowDialog(AlertModel alarmModel, bool isUseMultiScreen, bool isWarning = false)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().First().Close();

                isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().First().Close();

                var mainWindow = Application.Current.MainWindow;
                double width = mainWindow.ActualWidth;
                double height = mainWindow.ActualHeight;

                if (isUseMultiScreen)
                {
                    var validDisplays = DisplayHelpers.GetValidMonitors();

                    if(validDisplays.Count >= 2)
                    {
                        foreach (var validDisplay in validDisplays)
                        {
                            AlertNotifyView messageBoxEx1 = new AlertNotifyView()
                            {
                                WindowStartupLocation = WindowStartupLocation.Manual,
                                IsWarning = isWarning,
                                AlertModel = alarmModel,
                                Width = width * 0.6,
                                Height = height * 0.6,
                                Left = validDisplay.Left + validDisplay.Width * 0.2,
                                Top = validDisplay.Top + validDisplay.Height * 0.2
                            };
                            messageBoxEx1.Show();
                        }
                        return true;
                    }
                }

                AlertNotifyView messageBoxEx = new AlertNotifyView()
                {
                    IsWarning = isWarning,
                    AlertModel = alarmModel,
                    Width = width * 0.6,
                    Height = height * 0.6,
                };

                messageBoxEx.ShowDialog();
                return messageBoxEx.DialogResult;
            });
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                    if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().ToList().ForEach(anv => anv.Close());

                    isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                    if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().ToList().ForEach(mbe => mbe.Close());
                });
            }
            else
            {
                Close();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    bool isShown = Application.Current.Windows.OfType<AlertNotifyView>().Any();
                    if (isShown) Application.Current.Windows.OfType<AlertNotifyView>().ToList().ForEach(anv => anv.Close());

                    isShown = Application.Current.Windows.OfType<MessageBoxEx>().Any();
                    if (isShown) Application.Current.Windows.OfType<MessageBoxEx>().ToList().ForEach(mbe => mbe.Close());
                });
            }
        }
    }
}
