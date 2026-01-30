using OpenCvSharp;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace EQX.VisionUI.WPF.MVVM.Views.VisionToolEditParameterViews
{
    /// <summary>
    /// Interaction logic for AdaptiveThresholdToolEditParameterView.xaml
    /// </summary>
    public partial class AdaptiveThresholdToolEditParameterView : UserControl
    {
        public AdaptiveThresholdToolEditParameterView()
        {
            InitializeComponent();
            InitComboBox();
        }

        private void InitComboBox()
        {
            ObservableCollection<ThresholdTypes> ThresholdTypeList = new ObservableCollection<ThresholdTypes>();
            ObservableCollection<AdaptiveThresholdTypes> AdaptiveThresholdTypeList = new ObservableCollection<AdaptiveThresholdTypes>();

            var enumType = typeof(ThresholdTypes);
            var values = Enum.GetValues(enumType);

            foreach (var value in values)
            {
                ThresholdTypeList.Add((ThresholdTypes)value);
            }
            AdaptiveThresholdTypeList = new ObservableCollection<AdaptiveThresholdTypes>();
            var enumType1 = typeof(AdaptiveThresholdTypes);
            var values1 = Enum.GetValues(enumType1);

            foreach (var value in values1)
            {
                AdaptiveThresholdTypeList.Add((AdaptiveThresholdTypes)value);
            }

            cbAdaptiveThresholdTypes.ItemsSource = AdaptiveThresholdTypeList;
            cbThresholdTypes.ItemsSource = ThresholdTypeList;
        }
    }
}
