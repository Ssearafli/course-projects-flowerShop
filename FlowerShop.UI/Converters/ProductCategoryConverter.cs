using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FlowerShop.Domain;

namespace FlowerShop.UI.Converters
{
    public class ProductCategoryConverter: IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ProductCategory category)
            {
                return category switch
                {
                    ProductCategory.Flowers => "Цветы",
                    ProductCategory.Postcards => "Открытки",
                    ProductCategory.Packaging => "Упаковка",
                    ProductCategory.Toys => "Мягкие игрушки",
                    _ => category.ToString()
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
