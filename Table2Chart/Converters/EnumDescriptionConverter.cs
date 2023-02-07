using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Table2Chart.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            var descriptionAttr = fieldInfo
                .GetCustomAttributes(false)
                .OfType<DescriptionAttribute>()
                .Cast<DescriptionAttribute>()
                .SingleOrDefault();
            return descriptionAttr == null ? enumObj.ToString() : descriptionAttr.Description;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == string.Empty)
            {
                return value.ToString();
            }
            Enum myEnum = (Enum)value;
            string description = GetEnumDescription(myEnum);
            return description;
        }

        ///应该不会Bcak
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}