namespace EntityFramework.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
