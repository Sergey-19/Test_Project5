using EQX.Vision.Algorithms;
using System.Globalization;
using System.Windows.Data;

namespace EQX.VisionUI.WPF.Converters
{
    public class IdToPositionXConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is List<VisionToolDescription> visionToolDescriptions && values[1] is int id)
            {
                if (visionToolDescriptions.Count > 0 & visionToolDescriptions.FirstOrDefault(vtd => vtd.Id == id) != null)
                {
                    return visionToolDescriptions.FirstOrDefault(vtd => vtd.Id == id).Position.X;
                }
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IdToPositionYConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is List<VisionToolDescription> visionToolDescriptions && values[1] is int id)
            {
                if (visionToolDescriptions.Count > 0 & visionToolDescriptions.FirstOrDefault(vtd => vtd.Id == id) != null)
                {
                    return visionToolDescriptions.FirstOrDefault(vtd => vtd.Id == id).Position.Y;
                }
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
