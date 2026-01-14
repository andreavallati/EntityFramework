namespace EntityFramework.Entities
{
    public class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; } = string.Empty;
    }
}
