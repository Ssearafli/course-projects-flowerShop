namespace FlowerShop.Domain
{
    public enum ProductCategory
    {
        Flowers,
        Postcards,
        Packaging,
        Toys
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public Product Clone(Product product)
        {
            return new Product
            {
                ProductId = ProductId,
                Name = Name,
                Category = Category,
                Description = Description,
                Price = Price
            };
        }

    }
} 
