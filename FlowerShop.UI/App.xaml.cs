using System.Data;
using System.Windows;
using FlowerShop.Data.Interfaces;
using FlowerShop.Data.SqlServer;
using Microsoft.Extensions.Configuration;
using FlowerShop.Domain;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.UI
{
    public partial class App : Application
    {
        private IOrderRepository _orderRepository = null!;
        private IProductRepository _productRepository = null!;
        private FlowerShopDbContext _dbContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var factory = new FlowerShopDbContextFactory();
            _dbContext = factory.CreateDbContext(configuration);
            _dbContext.Database.Migrate();
            _productRepository = new ProductRepository(_dbContext);
            _orderRepository = new OrderRepository(_dbContext);

            var mainWindow = new MainWindow(_productRepository,_orderRepository);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }

}
