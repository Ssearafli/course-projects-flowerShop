using System;
using System.Collections.Generic;
using System.Text;
using FlowerShop.Domain;


namespace FlowerShop.Data.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        int Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
