using System;
using System.Collections.Generic;
using System.Text;
using FlowerShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Data.SqlServer
{
    public class FlowerShopDbContext: DbContext
    {
        public FlowerShopDbContext(DbContextOptions<FlowerShopDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
