using System;
using System.Collections;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace PersianInvoicing.Converters
{
    public class ItemRowIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null)
            {
                return string.Empty;
            }

            var item = values[0];
            var itemsControl = values[1] as ItemsControl;

            if (itemsControl?.ItemsSource is IList items)
            {
                int index = items.IndexOf(item);
                if (index != -1)
                {
                    return (index + 1).ToString();
                }
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}