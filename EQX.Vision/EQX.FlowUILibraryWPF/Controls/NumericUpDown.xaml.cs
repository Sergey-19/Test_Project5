using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace EQX.FlowUILibraryWPF.Controls
{
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(float), typeof(NumericUpDown), new PropertyMetadata());

        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ControlWidthProperty =
            DependencyProperty.Register("ControlWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.NaN));

        public double ControlWidth
        {
            get { return (double)GetValue(ControlWidthProperty); }
            set { SetValue(ControlWidthProperty, value); }
        }

        public static readonly DependencyProperty ControlHeightProperty =
            DependencyProperty.Register("ControlHeight", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.NaN));

        public double ControlHeight
        {
            get { return (double)GetValue(ControlHeightProperty); }
            set { SetValue(ControlHeightProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(float), typeof(NumericUpDown), new PropertyMetadata((float)1.0));

        public float Step
        {
            get { return (float)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(float), typeof(NumericUpDown), new PropertyMetadata(float.MinValue));

        public float MinValue
        {
            get { return (float)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(float), typeof(NumericUpDown), new PropertyMetadata(float.MaxValue));

        public float MaxValue
        {
            get { return (float)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(NumericUpDown), new PropertyMetadata(""));


        public NumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            Value = (Value >= MaxValue) ? MaxValue : Value + Step;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            Value = (Value <= MinValue) ? MinValue : Value - Step;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double FontSizeButton => ControlHeight * 30 / 100;
        public double WidthButton => ControlWidth * 10 / 100;
        public double WidthTxtBox => ControlWidth * 50 / 100;
        public double WidthLabelDescription => ControlWidth * 40 / 100;

    }
}
