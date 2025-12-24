using FlowerShop.Domain;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FlowerShop.UI
{
    public partial class SubscriptionWindow : Window
    {
        public Payment NewPayment { get; private set; }

        public SubscriptionWindow()
        {
            InitializeComponent();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var selectedItem = (ComboBoxItem)TariffComboBox.SelectedItem;
            int days = int.Parse(selectedItem.Tag.ToString());
            string name = selectedItem.Content.ToString().Split('-')[0].Trim();
            float price = days == 30 ? 299f : 2899f;

            NewPayment = new Payment
            {
                PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                SubscriptionExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(days)),
                TariffName = name,
                TariffAmount = price,
                PaymentId = Guid.NewGuid().ToString().Substring(0, 8) 
            };

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
            string cardNumber = CardNumberBox.Text.Replace(" ", "").Replace("-", "");
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                MessageBox.Show("Введите номер карты", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                CardNumberBox.Focus();
                return false;
            }

            if (cardNumber.Length < 16 || cardNumber.Length > 19 || !cardNumber.All(char.IsDigit))
            {
                MessageBox.Show("Номер карты должен содержать от 16 до 19 цифр", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                CardNumberBox.Focus();
                CardNumberBox.SelectAll();
                return false;
            }

            string expiry = ExpiryBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(expiry) || !Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/([0-9]{2})$"))
            {
                MessageBox.Show("Введите срок действия в формате ММ/ГГ (например, 05/26)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ExpiryBox.Focus();
                return false;
            }

            var parts = expiry.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse("20" + parts[1]);
            var expiryDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            if (expiryDate < DateTime.Now)
            {
                MessageBox.Show("Срок действия карты истек", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ExpiryBox.Focus();
                return false;
            }

            string cvc = CvcBox.Password;
            if (string.IsNullOrWhiteSpace(cvc) || cvc.Length != 3 || !cvc.All(char.IsDigit))
            {
                MessageBox.Show("Введите 3-значный код CVC/CVV (только цифры)", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                CvcBox.Focus();
                return false;
            }

            return true;
        }
    }
}