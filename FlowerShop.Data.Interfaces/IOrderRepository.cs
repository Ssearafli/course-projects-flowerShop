using System;
using System.Collections.Generic;
using System.Text;
using FlowerShop.Domain;

namespace FlowerShop.Data.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        int Add(Order order);
        bool Update(Order order);
        bool Delete(int id);
    }
}
