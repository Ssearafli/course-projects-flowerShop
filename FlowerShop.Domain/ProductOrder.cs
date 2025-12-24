using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerShop.Domain
{
    public class ProductOrder
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        [ForeignKey(nameof(ProductId))] public virtual Product Product { get; set; }
        [ForeignKey(nameof(OrderId))] public virtual Order Order { get; set; }
    }
}
