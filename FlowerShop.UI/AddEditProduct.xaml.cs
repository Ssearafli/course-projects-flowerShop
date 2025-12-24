using Azure.Core;
using FlowerShop.Domain;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FlowerShop.UI
{
    public partial class AddEditProduct : Window
    {
        public Product Product { get; private set; }
        public AddEditProduct(Product existing = null)
        {
            InitializeComponent();
            CategoryBox.ItemsSource = System.Enum.GetValues(typeof(ProductCategory));
            if (existing != null)
            {
                Product = existing;
                NameBox.Text = existing.Name;
                DescriptionBox.Text = existing.Description;
                PriceBox.Text = existing.Price.ToString();  
                CategoryBox.SelectedItem = existing.Category;
            }
            else
            {
                Product = new Product();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            Product.Name = NameBox.Text;
            Product.Description = DescriptionBox.Text;
            Product.Price = float.Parse(PriceBox.Text);

            Product.Category = (ProductCategory)CategoryBox.SelectedItem;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NameBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Введите описание товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                DescriptionBox.Focus();
                return false;
            }

            if (CategoryBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                CategoryBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PriceBox.Text))
            {
                MessageBox.Show("Введите цену товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PriceBox.Focus();
                return false;
            }

            if (!decimal.TryParse(PriceBox.Text.Replace(',', '.'),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PriceBox.Focus();
                PriceBox.SelectAll();
                return false;
            }

            return true;
        }
    }
}
