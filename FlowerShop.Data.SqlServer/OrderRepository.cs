using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;

namespace FlowerShop.Data.SqlServer
{
    public class OrderRepository:IOrderRepository
    {
        private readonly FlowerShopDbContext _context;
        public OrderRepository(FlowerShopDbContext context)
        {
            _context = context;
        }

        public int Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public bool Update(Order order)
        {
            var existing = _context.Orders.Find(order.OrderId);
            if (existing == null) 
                return false;
            existing.Clone(order);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}
