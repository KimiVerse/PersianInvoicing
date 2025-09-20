using System;
using System.Globalization;
using System.Windows.Data;

namespace PersianInvoicing.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue && parameter is string text)
            {
                var parts = text.Split('|');
                if (parts.Length == 2)
                {
                    return isTrue ? parts[0] : parts[1];
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}