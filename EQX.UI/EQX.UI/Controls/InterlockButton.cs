using EQX.Core.Interlock;
using System.Windows;
using System.Windows.Controls;

namespace EQX.UI.Controls
{
    public class InterlockButton : Button
    {
        public string? InterlockKey
        {
            get => (string?)GetValue(InterlockKeyProperty);
            set => SetValue(InterlockKeyProperty, value);
        }

        public static readonly DependencyProperty InterlockKeyProperty =
            DependencyProperty.Register(
                nameof(InterlockKey),
                typeof(string),
                typeof(InterlockButton),
                new PropertyMetadata(null, OnInterlockKeyChanged));

        public InterlockButton()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            IsEnabled = false;
            IsHitTestVisible = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InterlockService.Default.InterlockChanged += OnInterlockChanged;
            UpdateInterlockState();
            if (string.IsNullOrWhiteSpace(InterlockKey) == false)
            {
                InterlockService.Default.Reevaluate();
            }
        }

        private void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            InterlockService.Default.InterlockChanged -= OnInterlockChanged;
        }

        private void OnInterlockChanged(string key, bool satisfied)
        {
            Dispatcher.Invoke(() =>
            {
                if (string.Equals(key, InterlockKey, StringComparison.Ordinal))
                {
                    IsEnabled = satisfied;
                    IsHitTestVisible = satisfied;
                }
            });
        }

        private static void OnInterlockKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InterlockButton button)
            {
                button.UpdateInterlockState();
                if (button.IsLoaded && string.IsNullOrWhiteSpace(button.InterlockKey) == false)
                {
                    InterlockService.Default.Reevaluate();
                }
            }
        }

        private void UpdateInterlockState()
        {
            if (string.IsNullOrWhiteSpace(InterlockKey))
            {
                IsEnabled = true;
                IsHitTestVisible = true;
            }
            else
            {
                IsEnabled = false;
                IsHitTestVisible = false;
            }
        }
    }
}
