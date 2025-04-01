namespace EntityFramework.Entities
{
    // Customer and Orders (One-to-Many)
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;

        public Address Address { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
    }
}
