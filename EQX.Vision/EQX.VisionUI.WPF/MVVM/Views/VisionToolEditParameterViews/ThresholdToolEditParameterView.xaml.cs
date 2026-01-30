using OpenCvSharp;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace EQX.VisionUI.WPF.MVVM.Views.VisionToolEditParameterViews
{
    /// <summary>
    /// Interaction logic for ThresholdToolEditParameterView.xaml
    /// </summary>
    public partial class ThresholdToolEditParameterView : UserControl
    {
        public ThresholdToolEditParameterView()
        {
            InitializeComponent();
            InitComboBox();
        }

        private void InitComboBox()
        {
            ObservableCollection<ThresholdTypes> ThresholdTypeList = new ObservableCollection<ThresholdTypes>();
            var enumType = typeof(ThresholdTypes);
            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                ThresholdTypeList.Add((ThresholdTypes)value);
            }
            cbThresholdTypes.ItemsSource = ThresholdTypeList;
        }
    }
}
