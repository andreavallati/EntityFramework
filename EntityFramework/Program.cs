using EntityFramework.Context;
using EntityFramework.Entities;
using EntityFramework.Enums;
using EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core - E-Commerce Sample ===\n");

            using var context = new ECommerceContext();

            // Apply pending migrations
            await context.Database.MigrateAsync();
            Console.WriteLine();
            Console.WriteLine("Database initialized with migrations\n");

            // Verify seed data loaded correctly
            var customerCount = await context.Customers.CountAsync();
            var orderCount = await context.Orders.CountAsync();
            Console.WriteLine();
            Console.WriteLine($"[DEBUG] Database has {customerCount} customers and {orderCount} orders\n");

            // ========== 1. COMPILED QUERIES ==========
            Console.WriteLine("--- 1. Compiled Queries ---");

            var electronicsCategory = await CompiledQueries.GetCategoryWithProductsAsync(context, 1);
            if (electronicsCategory != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Category: {electronicsCategory.Name}");
                foreach (var product in electronicsCategory.Products)
                {
                    Console.WriteLine($"Product: {product.Name} - Price: {product.Price}");
                }
            }

            var customer = CompiledQueries.GetCustomerWithOrdersSync(context, 1);
            if (customer != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Customer: {customer.FullName}");
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($"Order #{order.OrderId} placed on {order.OrderDate}");
                }
            }
            Console.WriteLine();

            // ========== 2. CHANGE TRACKING DEMONSTRATION ==========
            Console.WriteLine("--- 2. Change Tracking ---");

            var trackedProduct = await context.Products.FirstAsync(p => p.ProductId == 1);
            var originalPrice = trackedProduct.Price;
            trackedProduct.Price = 899.99m;

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .ToList();

            Console.WriteLine();
            foreach (var entry in entries)
            {
                Console.WriteLine($"Modified Entity: {entry.Entity.GetType().Name}");
                foreach (var prop in entry.Properties.Where(p => p.IsModified))
                {
                    Console.WriteLine($"{prop.Metadata.Name}: Old value: {prop.OriginalValue} - New value: {prop.CurrentValue}");
                }
            }
            await context.SaveChangesAsync();
            Console.WriteLine("Changes saved\n");

            // ========== 3. NO TRACKING QUERIES (Read-Only) ==========
            Console.WriteLine("--- 3. AsNoTracking (Read-Only Queries) ---");

            var untrackedProducts = await context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == 1)
                .ToListAsync();

            Console.WriteLine();
            Console.WriteLine($"Retrieved {untrackedProducts.Count} products (not tracked)");
            Console.WriteLine();

            // ========== 4. TRANSACTIONS ==========
            Console.WriteLine("--- 4. Transactions ---");

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

                var newOrder = new Order
                {
                    CustomerId = newCustomer.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 199.99m,
                    Status = OrderStatus.Pending
                };
                context.Orders.Add(newOrder);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                Console.WriteLine();
                Console.WriteLine("Transaction committed successfully\n");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine();
                Console.WriteLine($"Transaction rolled back: {ex.Message}\n");
            }

            // ========== 5. COMPLEX QUERIES & PROJECTIONS ==========
            Console.WriteLine("--- 5. Complex Queries & Projections ---");

            var productStats = await context.Categories
                .Select(category => new
                {
                    CategoryName = category.Name,
                    ProductCount = category.Products.Count,
                    AveragePrice = category.Products.Any() ? category.Products.Average(p => p.Price) : 0
                })
                .ToListAsync();

            Console.WriteLine();
            foreach (var stat in productStats)
            {
                Console.WriteLine($"{stat.CategoryName}: {stat.ProductCount} products, Avg ${stat.AveragePrice:F2}");
            }
            Console.WriteLine();

            // ========== 6. Include & ThenInclude (Eager Loading) ==========
            Console.WriteLine("--- 6. Eager Loading (Include/ThenInclude) ---");

            var ordersWithDetails = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderDate >= DateTime.UtcNow.AddMonths(-1))
                .ToListAsync();

            Console.WriteLine();
            foreach (var order in ordersWithDetails)
            {
                var total = order.OrderItems.Sum(oi => oi.Product.Price * oi.Quantity);
                Console.WriteLine($"Order #{order.OrderId} by {order.Customer.FullName} - Status: {order.Status}");
                Console.WriteLine($"Items: {order.OrderItems.Count}, Total: ${total:F2}");
            }
            Console.WriteLine();

            // ========== 7. QUERY FILTERS (Soft Delete) ==========
            Console.WriteLine("--- 7. Query Filters (Soft Delete Pattern) ---");

            var activeProducts = await context.Products.CountAsync();
            Console.WriteLine();
            Console.WriteLine($"Active products: {activeProducts}");

            // Soft delete a product
            var productToDelete = await context.Products.FirstAsync(p => p.ProductId == 4);
            productToDelete.IsDeleted = true;
            await context.SaveChangesAsync();

            var activeProductsAfterDelete = await context.Products.CountAsync();
            Console.WriteLine();
            Console.WriteLine($"Active products after soft delete: {activeProductsAfterDelete}");

            // Query including soft-deleted items
            var allProducts = await context.Products.IgnoreQueryFilters().CountAsync();
            Console.WriteLine();
            Console.WriteLine($"All products (including soft-deleted): {allProducts}\n");

            // ========== 8. CONCURRENCY HANDLING ==========
            Console.WriteLine("--- 8. Optimistic Concurrency ---");

            try
            {
                using var context1 = new ECommerceContext();
                using var context2 = new ECommerceContext();

                var product1 = await context1.Products.FirstAsync(p => p.ProductId == 1);
                var product2 = await context2.Products.FirstAsync(p => p.ProductId == 1);

                product1.Price = 999.99m;
                await context1.SaveChangesAsync();
                Console.WriteLine();
                Console.WriteLine("Context 1 saved successfully");

                product2.Price = 1099.99m;
                await context2.SaveChangesAsync();
                Console.WriteLine();
                Console.WriteLine("Context 2 saved successfully");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Concurrency conflict detected: {ex.Message}");
                Console.WriteLine("Conflict resolution required!\n");
            }

            // ========== 9. RAW SQL QUERIES ==========
            Console.WriteLine("--- 9. Raw SQL Queries ---");

            var categoryId = 1;
            var productsFromSql = await context.Products
                .FromSqlInterpolated($"SELECT * FROM Products WHERE CategoryId = {categoryId} AND IsDeleted = 0")
                .ToListAsync();
            Console.WriteLine();
            Console.WriteLine($"Products from raw SQL: {productsFromSql.Count}");
            Console.WriteLine();

            // ========== 10. SPLIT QUERIES ==========
            Console.WriteLine("--- 10. Split Queries (AsSplitQuery) ---");

            var categoriesWithProducts = await context.Categories
                .Include(c => c.Products)
                .AsSplitQuery()
                .ToListAsync();
            Console.WriteLine();
            Console.WriteLine($"Loaded {categoriesWithProducts.Count} categories with split queries\n");

            // ========== 11. GROUPING & AGGREGATIONS ==========
            Console.WriteLine("--- 11. Grouping & Aggregations ---");

            var ordersByStatus = await context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            Console.WriteLine();
            foreach (var group in ordersByStatus)
            {
                Console.WriteLine($"{group.Status}: {group.Count} orders, Total: ${group.TotalAmount:F2}");
            }
            Console.WriteLine();

            // ========== 12. TABLE-PER-HIERARCHY (TPH) Inheritance ==========
            Console.WriteLine("--- 12. TPH Inheritance (Payments) ---");

            Console.WriteLine();
            var allPayments = await context.Payments.ToListAsync();
            foreach (var payment in allPayments)
            {
                var paymentType = payment switch
                {
                    CreditCardPayment cc => $"Credit Card ending in {cc.CardNumber.Substring(Math.Max(0, cc.CardNumber.Length - 4))}",
                    PayPalPayment pp => $"PayPal ({pp.PayPalEmail})",
                    _ => "Unknown"
                };
                Console.WriteLine($"Payment #{payment.PaymentId}: {paymentType} - ${payment.Amount:F2}");
            }
            Console.WriteLine();

            Console.WriteLine("=== Demo Complete ===");
        }
    }
}