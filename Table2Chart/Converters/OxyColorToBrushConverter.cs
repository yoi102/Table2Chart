using OxyPlot;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Table2Chart.Common.Models.OxyModels.Color;

namespace Table2Chart.Converters
{
    [ValueConversion(typeof(OxyColor), typeof(Brush))]
    public class OxyColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string propName)
            {
                OxyColor color = (OxyColor)typeof(MyColors).GetProperty(propName).GetValue(value, null);

                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is SolidColorBrush brush)
            //{
            //    return OxyColor.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
            //}
            return OxyColors.Automatic;
        }
    }
}