using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            if (!optionsBuilder.IsConfigured)
            {
                // Read connection string from configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? @"Server=(localdb)\MSSQLLocalDB;Database=ECommerceDb;Trusted_Connection=True;";

                optionsBuilder.UseSqlServer(connectionString);

#if DEBUG
                // Enable sensitive data logging and detailed errors for development ONLY
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
#endif

                // Configure logging
                optionsBuilder.LogTo(Console.WriteLine, new[] {
                    DbLoggerCategory.Database.Command.Name
                }, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceContext).Assembly);
        }
    }
}
