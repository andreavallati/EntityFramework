using EntityFramework.Context;
using EntityFramework.Entities;
using EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var context = new ECommerceContext();

            // Ensure database is created and seed data is added
            await context.Database.EnsureCreatedAsync();

            // Query using Compiled Query (Async)
            var electronicsCategory = await CompiledQueries.GetCategoryWithProductsAsync(context, 1);
            if (electronicsCategory != null)
            {
                Console.WriteLine($"Category: {electronicsCategory.Name}");
                foreach (var product in electronicsCategory.Products)
                {
                    Console.WriteLine($"  Product: {product.Name} - ${product.Price}");
                }
            }

            // Query using Compiled Query (Sync)
            var customer = CompiledQueries.GetCustomerWithOrdersSync(context, 1);
            if (customer != null)
            {
                Console.WriteLine($"Customer: {customer.FullName}");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"  Order #{order.OrderId} placed on {order.OrderDate}");
                }
            }

            // Transaction Example
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var newCustomer = new Customer
                {
                    FullName = "Alice Johnson",
                    Address = new Address { Street = "456 Oak St", City = "Denver", PostalCode = "80202" }
                };
                context.Customers.Add(newCustomer);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                Console.WriteLine("Transaction committed.");
            }
            catch
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Transaction rolled back.");
            }

            // 1. **Complex Query**: Group Products by Category and compute stats
            var productStats = await context.Categories
                .Select(category => new
                {
                    CategoryName = category.Name,
                    ProductCount = category.Products.Count,
                    AveragePrice = category.Products.Any() ? category.Products.Average(p => p.Price) : 0
                })
                .ToListAsync();

            Console.WriteLine("\nProduct Stats:");
            foreach (var stat in productStats)
            {
                Console.WriteLine($"Category: {stat.CategoryName}, Products: {stat.ProductCount}, Avg Price: {stat.AveragePrice:C}");
            }

            // 2. **Raw SQL Query**: Fetch Top Customers by Total Spending (Using View)
            var topCustomers = await context.TopCustomers
                .FromSqlInterpolated($"SELECT * FROM View_TopCustomersBySpending")
                .ToListAsync();

            Console.WriteLine("\nTop Customers by Spending:");
            foreach (var topCustomer in topCustomers)
            {
                Console.WriteLine($"Customer: {topCustomer.CustomerName}, Total Spent: {topCustomer.TotalSpent:C}");
            }

            // 3. **Raw SQL Insert/Update**: Add a new product to the database
            var rowsAffected = await context.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO Products (Name, Price, CategoryId) VALUES ('New Product', 99.99, 1)");
            Console.WriteLine($"\nRows affected by INSERT: {rowsAffected}");

            // 4. **Advanced Query**: Fetch Orders with Nested Data
            var ordersWithDetails = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderDate >= DateTime.UtcNow.AddMonths(-1))
                .Select(order => new
                {
                    order.OrderId,
                    CustomerName = order.Customer.FullName,
                    TotalAmount = order.OrderItems.Sum(oi => oi.Product.Price * oi.Quantity),
                    Products = order.OrderItems.Select(oi => oi.Product.Name).ToList()
                })
                .ToListAsync();

            Console.WriteLine("\nOrders with Details:");
            foreach (var order in ordersWithDetails)
            {
                Console.WriteLine($"Order #{order.OrderId} by {order.CustomerName}, Total: {order.TotalAmount:C}");
                Console.WriteLine("Products: " + string.Join(", ", order.Products));
            }

            // 5. **Transaction with Raw SQL**: Update Product Prices within a Transaction
            using var rawTransaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Raw SQL Update
                rowsAffected = await context.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE Products SET Price = Price * 1.10 WHERE CategoryId = 1");
                Console.WriteLine($"\nRows affected by Price Update: {rowsAffected}");

                // Simulate another operation in the transaction
                var newCategory = new Category { Name = "New Category" };
                context.Categories.Add(newCategory);
                await context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
                Console.WriteLine("Transaction committed successfully.");
            }
            catch
            {
                // Rollback transaction
                await transaction.RollbackAsync();
                Console.WriteLine("Transaction rolled back due to an error.");
            }
        }
    }
}