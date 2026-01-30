using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Globalization;
using System.Windows.Data;

namespace EQX.VisionUI.WPF.Converters
{
    public class MatToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Mat mat)
            {
                try
                {
                    return mat.ToBitmapSource();
                }
                catch(Exception ex) 
                {
                }
            }

            return Binding.DoNothing;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }

}
