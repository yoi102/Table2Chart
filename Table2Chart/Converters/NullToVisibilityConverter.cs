using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Table2Chart.Converters
{
    /// <summary>
    /// 对象为Null 对象转为 Visible 或Collapsed
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)parameter)
            {
                case "0":
                    if (value == null)
                        return Visibility.Visible; break;
                case "1":
                    if (value != null)
                        return Visibility.Collapsed; break;
                default:
                    break;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}