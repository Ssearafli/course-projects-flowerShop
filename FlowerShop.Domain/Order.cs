using System.Xml.Linq;

namespace FlowerShop.Domain
{
    public enum OrderStatus
    {
        New,
        In_process,
        Completed,
        Cancelled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public float TotalPrice {  get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly OrderDate { get; set; }
        public Order Clone(Order order)
        {
            return new Order
            {
                OrderId=OrderId,
                CustomerName=CustomerName,
                CustomerPhone=CustomerPhone,
                TotalPrice=TotalPrice,
                Status=Status,
                OrderDate=OrderDate
            };
        }
    }
}
