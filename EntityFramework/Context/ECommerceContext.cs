using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context
{
    public class ECommerceContext : DbContext
    {
        // DbSet for each entity
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<TopCustomer> TopCustomers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\\YourInstance;Database=YourDatabase;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many: Category -> Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // One-to-Many: Customer -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            // Many-to-Many: Order -> Products via OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Configure JSON column
            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.Address);

            // Configure Table-Per-Hierarchy for Payments
            modelBuilder.Entity<Payment>()
                .UseTphMappingStrategy()
                .HasDiscriminator<string>("PaymentType")
                .HasValue<CreditCardPayment>("CreditCard")
                .HasValue<PayPalPayment>("PayPal");

            // Configure View
            modelBuilder.Entity<TopCustomer>()
                .ToView("View_TopCustomersBySpending")
                .HasNoKey();

            // Index Example
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics" },
                new Category { CategoryId = 2, Name = "Books" },
                new Category { CategoryId = 3, Name = "Clothing" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Smartphone", Price = 799.99m, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Laptop", Price = 1299.99m, CategoryId = 1 },
                new Product { ProductId = 3, Name = "Fiction Book", Price = 19.99m, CategoryId = 2 },
                new Product { ProductId = 4, Name = "T-Shirt", Price = 29.99m, CategoryId = 3 }
            );
        }
    }
}
