using Azure.Core;
using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlowerShop.UI
{
    public partial class MainWindow : Window
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Order> Orders{ get; set; }

        public MainWindow(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            LoadData();
            DataContext = this;
        }
        private void LoadData()
        {
            if (_productRepository != null && _orderRepository != null)
            {
                Products = new ObservableCollection<Product>(_productRepository.GetAll());
                Orders = new ObservableCollection<Order>(_orderRepository.GetAll());

                ProductGrid.ItemsSource = Products;
                OrderGrid.ItemsSource = Orders;
            }
        }
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}