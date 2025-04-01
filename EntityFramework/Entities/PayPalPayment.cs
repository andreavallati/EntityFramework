namespace EntityFramework.Entities
{
    public class PayPalPayment : Payment
    {
        public string PayPalEmail { get; set; } = string.Empty;
    }
}
