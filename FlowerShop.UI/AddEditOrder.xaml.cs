using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlowerShop.UI
{
    public partial class AddEditOrder : Window
    {
        public Order Order { get; private set; }
        private readonly IProductRepository _productRepository;
        private ObservableCollection<ProductOrder> _orderItems;

        public AddEditOrder(IProductRepository productRepository, Order existing = null)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderItems = new ObservableCollection<ProductOrder>();
            ProductsGrid.DataContext = _orderItems;
            InitializeControls();

            if (existing != null)
            {
                LoadExistingOrder(existing);
            }
            else
            {
                InitializeNewOrder();
            }

            UpdateTotalPrice();
        }

        private void InitializeControls()
        {
            StatusBox.ItemsSource = Enum.GetValues(typeof(OrderStatus));
            var products = _productRepository.GetAll();
            ProductComboBox.ItemsSource = products;

            if (products.Any())
            {
                ProductComboBox.SelectedIndex = 0;               
            }
        }

        private void LoadExistingOrder(Order existing)
        {
            Order = existing;
            NameBox.Text = existing.CustomerName;
            PhoneBox.Text = existing.CustomerPhone;
            StatusBox.SelectedItem = existing.Status;
            OrderDatePicker.SelectedDate = new DateTime(existing.OrderDate.Year,
                existing.OrderDate.Month, existing.OrderDate.Day);
            if (existing.ProductOrders != null)
            {
                foreach (var item in existing.ProductOrders)
                {
                    _orderItems.Add(new ProductOrder
                    {
                        ProductId = item.ProductId,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }
            }
        }

        private void InitializeNewOrder()
        {
            Order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                Status = OrderStatus.New
            };

            OrderDatePicker.SelectedDate = DateTime.Today;
            StatusBox.SelectedItem = OrderStatus.New;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is Product selectedProduct)
            {
                if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
                {
                    var existingItem = _orderItems.FirstOrDefault(oi => oi.ProductId == selectedProduct.ProductId);

                    if (existingItem != null)
                    {
                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        _orderItems.Add(new ProductOrder
                        {
                            ProductId = selectedProduct.ProductId,
                            Product = selectedProduct,
                            Quantity = quantity,
                            UnitPrice = selectedProduct.Price
                        });
                    }

                    UpdateTotalPrice();
                    QuantityTextBox.Text = "1";
                }
                else
                {
                    MessageBox.Show("Введите корректное количество (больше 0)", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    QuantityTextBox.Focus();
                }
            }
        }

        private void RemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProductOrder item)
            {
                _orderItems.Remove(item);
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            float total = _orderItems.Sum(item => item.Quantity * item.UnitPrice);
            TotalPriceText.Text = $"{total} руб.";
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {          
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;
            Order.CustomerName = NameBox.Text.Trim();
            Order.CustomerPhone = PhoneBox.Text.Trim();
            Order.Status = (OrderStatus)StatusBox.SelectedItem;

            if (OrderDatePicker.SelectedDate.HasValue)
            {
                Order.OrderDate = DateOnly.FromDateTime(OrderDatePicker.SelectedDate.Value);
            }
            Order.ProductOrders = _orderItems.ToList();
            Order.TotalPrice = _orderItems.Sum(item => item.Quantity * item.UnitPrice);
            DialogResult = true;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите имя клиента", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NameBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PhoneBox.Focus();
                return false;
            }

            if (StatusBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус заказа", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                StatusBox.Focus();
                return false;
            }

            if (_orderItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один товар в заказ", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ProductComboBox.Focus();
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}