using EntityFramework.Enums;

namespace EntityFramework.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new();

        // One-to-One relationship with Payment
        public Payment? Payment { get; set; }
    }
}
