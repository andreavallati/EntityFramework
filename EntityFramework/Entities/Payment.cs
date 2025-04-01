namespace EntityFramework.Entities
{
    // Base class for Payment (TPH)
    public abstract class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
