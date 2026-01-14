namespace EntityFramework.Entities
{
    // Product and Category (One-to-Many)
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public List<Product> Products { get; set; } = new();
    }
}
