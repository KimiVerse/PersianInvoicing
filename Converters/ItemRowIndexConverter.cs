using System;
using System.Globalization;
using System.Windows.Data;

namespace PersianInvoicing.Converters
{
    public class ItemRowIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrameworkElement element && element.DataContext is object item && element.BindingContext is DataGrid dataGrid)
            {
                var index = dataGrid.Items.IndexOf(item);
                return (index + 1).ToString();
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}