using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<ProductOrder> ProductOrders {get; set;} = new List<ProductOrder>();

        [NotMapped]
        public string OrderDetails
        {
            get
            {
                if (ProductOrders == null || !ProductOrders.Any())
                    return "Нет товаров";

                return string.Join(", ", ProductOrders.Select(po =>
                    $"{po.Product?.Name} x{po.Quantity}"));
            }
        }
        public Order Clone(Order orderToClone)
        {
            return new Order
            {
                OrderId = orderToClone.OrderId,
                CustomerName = orderToClone.CustomerName,
                CustomerPhone = orderToClone.CustomerPhone,
                TotalPrice = orderToClone.TotalPrice,
                Status = orderToClone.Status,
                OrderDate = orderToClone.OrderDate,
                ProductOrders = orderToClone.ProductOrders?.Select(po => new ProductOrder
                {
                    Id = po.Id,
                    ProductId = po.ProductId,
                    OrderId = po.OrderId,
                    Quantity = po.Quantity,
                    UnitPrice = po.UnitPrice                 
                }).ToList()
            };
        }
    }
}
