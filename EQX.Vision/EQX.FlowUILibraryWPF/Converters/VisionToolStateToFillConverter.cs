using EQX.Core.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace EQX.FlowUILibraryWPF.Converters
{
    public class VisionToolStateToFillConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ERunState)
            {
                switch (value)
                {
                    case ERunState.Idle:
                        return Brushes.Red;
                    case ERunState.Running:
                        return Brushes.Yellow;
                    case ERunState.Done:
                        return Brushes.Green;
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
