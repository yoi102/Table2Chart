using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Table2Chart.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class ZeroIntToUnVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out int result))
            {
                if (result == 0)
                    return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}