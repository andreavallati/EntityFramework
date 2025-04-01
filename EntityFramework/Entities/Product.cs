namespace EntityFramework.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Foreign key and navigation property
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
