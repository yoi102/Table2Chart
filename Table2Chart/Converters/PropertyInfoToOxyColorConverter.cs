using OxyPlot;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using Table2Chart.Common.Models.OxyModels.Color;

namespace Table2Chart.Converters
{
    [ValueConversion(typeof(OxyColor), typeof(Brush))]
    public class PropertyInfoToOxyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OxyColor prop)
            {
                foreach (var item in typeof(MyColors).GetProperties())
                {
                    if (prop == (OxyColor)typeof(MyColors).GetProperty(item.Name).GetValue(item, null))
                    {
                        return item;
                    };
                }
            }
            return typeof(MyColors).GetProperty("Random");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PropertyInfo prop)
            {
                OxyColor color = (OxyColor)typeof(MyColors).GetProperty(prop.Name).GetValue(value, null);

                return color;
            }
            return OxyColors.Automatic;
        }
    }
}