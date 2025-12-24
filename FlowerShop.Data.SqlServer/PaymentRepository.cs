using FlowerShop.Data.Interfaces;
using FlowerShop.Domain;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Data.SqlServer
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly FlowerShopDbContext _context;
        public PaymentRepository(FlowerShopDbContext context)
        {
            _context = context;
        }

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public List<Payment> GetAll()
        {
            return _context.Payments
                .OrderByDescending(p => p.PaymentDate)
                .ToList();
        }

        public Payment GetLatest()
        {
            return _context.Payments
                .OrderByDescending(p => p.SubscriptionExpiryDate)
                .ThenByDescending(p => p.Id)
                .FirstOrDefault();
        }
    }
}
