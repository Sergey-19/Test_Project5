using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EQX.VisionUI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new PropertyMetadata());

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty LabelDescriptionWidthProperty =
            DependencyProperty.Register("LabelDescriptionWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.NaN));

        public double LabelDescriptionWidth
        {
            get { return (double)GetValue(LabelDescriptionWidthProperty); }
            set { SetValue(LabelDescriptionWidthProperty, value); }
        }

        public static readonly DependencyProperty TextBoxValueWidthProperty =
            DependencyProperty.Register("TextBoxValueWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.NaN));

        public double TextBoxValueWidth
        {
            get { return (double)GetValue(TextBoxValueWidthProperty); }
            set { SetValue(TextBoxValueWidthProperty, value); }
        }

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.NaN));

        public double ButtonWidth
        {
            get { return (double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
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


        public static readonly DependencyProperty ValidationRuleProperty =
            DependencyProperty.Register(nameof(ValidationRule), typeof(ValidationRule), typeof(NumericUpDown), new PropertyMetadata(null, OnValidationRuleChanged));

        public ValidationRule ValidationRule
        {
            get => (ValidationRule)GetValue(ValidationRuleProperty);
            set => SetValue(ValidationRuleProperty, value);
        }

        private static void OnValidationRuleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown numericUpDown && e.NewValue is ValidationRule validationRule)
            {
                var binding = BindingOperations.GetBinding(numericUpDown.txtNum, TextBox.TextProperty);
                if (binding != null)
                {
                    binding.ValidationRules.Clear();
                    binding.ValidationRules.Add(validationRule);
                }
            }
        }
        public NumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            Value = Value >= MaxValue ? MaxValue : Value + Step;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            Value = Value <= MinValue ? MinValue : Value - Step;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void txtNum_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text) && e.Text != "." && e.Text != "-";
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }

}
