using System;
using System.Collections.Generic;
using System.Text;
using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;

namespace FlowerShop.Data.SqlServer
{
    public class ProductRepository: IProductRepository
    {
        private readonly FlowerShopDbContext _context;
        public ProductRepository(FlowerShopDbContext context)
        {
            _context = context;
        }

        public int Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product.ProductId;
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public bool Update(Product product)
        {
            var existing = _context.Products.Find(product.ProductId);
            if (existing == null)
            {
                return false;
            }
            existing.Clone(product);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return false;
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
