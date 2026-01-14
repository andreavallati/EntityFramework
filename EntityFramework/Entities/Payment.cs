namespace EntityFramework.Entities
{
    // Base class for Payment (TPH)
    public abstract class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // One-to-One relationship with Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
