using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EQX.Vision.App.Converters
{
    public class MatToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is Mat)
                {
                    return WriteableBitmapConverter.ToWriteableBitmap((Mat)value);
                }
            }
            catch
            {
                return Binding.DoNothing;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BitmapSourceConverter.ToMat(value as BitmapImage);
        }
    }
}
