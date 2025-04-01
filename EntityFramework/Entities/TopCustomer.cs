namespace EntityFramework.Entities
{
    // View Example: Top Customers by Spending
    public class TopCustomer
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }
}
