using System;
using System.Collections.Generic;
using System.Linq;
using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Data.SqlServer
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FlowerShopDbContext _context;
        public OrderRepository(FlowerShopDbContext context)
        {
            _context = context;
        }

        public int Add(Order order)
        {
            order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Orders.Add(order);

            if (order.ProductOrders != null && order.ProductOrders.Any())
            {
                foreach (var item in order.ProductOrders)
                {
                    item.OrderId = order.OrderId;
                }
            }

            _context.SaveChanges();
            return order.OrderId;
        }

        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
                //.OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                .Include(o => o.ProductOrders)
                .ThenInclude(po => po.Product)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public bool Update(Order order)
        {
            var existing = GetById(order.OrderId);
            if (existing == null)
                return false;

            existing.CustomerName = order.CustomerName;
            existing.CustomerPhone = order.CustomerPhone;
            existing.Status = order.Status;
            existing.TotalPrice = order.TotalPrice;

            if (order.ProductOrders != null)
            {

                var existingItems = _context.ProductOrders.Where(po => po.OrderId == order.OrderId);
                _context.ProductOrders.RemoveRange(existingItems);

                foreach (var item in order.ProductOrders)
                {
                    item.OrderId = order.OrderId;
                    item.Id = 0; 
                }
                _context.ProductOrders.AddRange(order.ProductOrders);
            }

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var order = GetById(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}