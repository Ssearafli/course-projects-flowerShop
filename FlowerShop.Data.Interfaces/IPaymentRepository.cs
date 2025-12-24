using FlowerShop.Domain;

namespace FlowerShop.Data.Interfaces
{
    public interface IPaymentRepository
    {
        void Add(Payment payment);
        List<Payment> GetAll();
        Payment GetLatest(); 
    }
}
