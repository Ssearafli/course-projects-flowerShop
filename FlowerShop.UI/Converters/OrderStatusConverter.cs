using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FlowerShop.Domain;

namespace FlowerShop.UI.Converters
{
    public class OrderStatusConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                return status switch
                {
                    OrderStatus.New => "Новый",
                    OrderStatus.In_process => "В процессе",
                    OrderStatus.Completed => "Завершен",
                    OrderStatus.Cancelled => "Отменен",
                    _ => status.ToString()
                };
            }
            return DependencyProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
