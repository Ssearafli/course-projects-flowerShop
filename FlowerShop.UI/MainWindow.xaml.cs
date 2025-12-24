using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;
using System.Collections.ObjectModel;
using System.Windows;

namespace FlowerShop.UI
{
    public partial class MainWindow : Window
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IPaymentRepository _paymentRepository;

        public ObservableCollection<Product> Products { get; set; } = [];
        public ObservableCollection<Order> Orders{ get; set; } = [];

        public MainWindow(IProductRepository productRepository, IOrderRepository orderRepository, IPaymentRepository paymentRepository)
        {
            InitializeComponent();
            DataContext = this;

            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;

            ProductGrid.ItemsSource = Products;
            OrderGrid.ItemsSource = Orders;

            LoadData();
            UpdateSubscriptionUI();
        }
        private void LoadData()
        {
            if (_productRepository != null && _orderRepository != null)
            {
                LoadProducts();
                LoadOrders();             
            }
            
        }      

        private void LoadProducts()
        {
            List<Product> products = _productRepository.GetAll();
            Products.Clear();
            foreach (Product product in products)
            {
                Products.Add(product);
            }
        }

        private void LoadOrders()
        {
            List<Order> orders = _orderRepository.GetAll();
            Orders.Clear();
            foreach (Order order in orders)
            {
                Orders.Add(order);
            }
        }
        private bool IsSubscriptionActive()
        {
            var lastPayment = _paymentRepository.GetLatest();
            if (lastPayment == null) return false;

            return lastPayment.SubscriptionExpiryDate >= DateOnly.FromDateTime(DateTime.Now);
        }

        private void UpdateSubscriptionUI()
        {
            var last = _paymentRepository.GetLatest();
            if (last != null && last.SubscriptionExpiryDate >= DateOnly.FromDateTime(DateTime.Now))
            {
                SubscriptionNameText.Text = last.TariffName;
                SubscriptionPriceText.Text = $"{last.TariffAmount} руб.";
                SubscriptionStatusText.Text = "Активна";

                var remaining = last.SubscriptionExpiryDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
                DaysRemainingText.Text = $"{remaining} дней";
            }
            else
            {
                SubscriptionStatusText.Text = "Истекла или отсутствует";
                DaysRemainingText.Text = "0 дней";
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var form = new AddEditProduct();
            form.Owner = this;
            if (form.ShowDialog() == true && form.Product != null)
            {
                _productRepository.Add(form.Product);
                LoadProducts();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ProductGrid.SelectedItem is Product selectedProduct)
            {
                var form = new AddEditProduct(selectedProduct);
                form.Owner = this;

                if (form.ShowDialog() == true && form.Product != null)
                {
                    try
                    {
                        if (_productRepository.Update(form.Product))
                        {
                            LoadProducts();
                            MessageBox.Show("Товар успешно обновлен", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить товар", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении товара: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ProductGrid.SelectedItem == null) MessageBox.Show("Выберите товар для удаления");
            else
            {
                Product selected = (Product)ProductGrid.SelectedItem;
                _productRepository.Delete(selected.ProductId);
                LoadProducts();
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var form = new AddEditOrder(_productRepository);
            form.Owner = this;
            if (form.ShowDialog() == true && form.Order != null)
            {
                _orderRepository.Add(form.Order);
                LoadOrders();
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                var form = new AddEditOrder(_productRepository, selectedOrder);
                form.Owner = this;

                if (form.ShowDialog() == true && form.Order != null)
                {
                    try
                    {
                        if (_orderRepository.Update(form.Order))
                        {
                            LoadOrders();
                            MessageBox.Show("Заказ успешно обновлен", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить заказ", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении заказа: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubscriptionActive())
            {
                MessageBox.Show("Для добавления товаров необходима активная подписка!", "Доступ ограничен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OrderGrid.SelectedItem == null) MessageBox.Show("Выберите товар для удаления");
            else
            {
                Order selected = (Order)OrderGrid.SelectedItem;
                _orderRepository.Delete(selected.OrderId);
                LoadOrders();
            }
        }

        private void SubscriptionHistory_Click(object sender, RoutedEventArgs e)
        {
            var history = _paymentRepository.GetAll();
            string report = string.Join("\n", history.Select(p => $"{p.PaymentDate}: {p.TariffName} ({p.TariffAmount}р)"));
            MessageBox.Show(report, "История платежей");
        }

        private void Subscription_Click(object sender, RoutedEventArgs e)
        {
            if (IsSubscriptionActive())
            {
                MessageBox.Show("У вас уже есть активная подписка. Дождитесь её окончания, чтобы оформить новую.",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return; 
            }

            var subWin = new SubscriptionWindow();
            subWin.Owner = this;

            if (subWin.ShowDialog() == true && subWin.NewPayment != null)
            {
                try
                {
                    _paymentRepository.Add(subWin.NewPayment);
                    UpdateSubscriptionUI(); 
                    MessageBox.Show("Подписка успешно оформлена!", "Успех",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}