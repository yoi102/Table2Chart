using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Table2Chart.Converters
{
    /// <summary>
    /// 反向的bool转visibility
    /// </summary>
    [Obsolete("未使用")]
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b) return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
            {
                if (v != Visibility.Visible) return true;
            }
            return false;
        }
    }
}