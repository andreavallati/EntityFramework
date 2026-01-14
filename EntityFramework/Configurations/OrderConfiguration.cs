using EntityFramework.Entities;
using EntityFramework.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // Value converter for enum to string
            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Relationship with Customer
            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One relationship with Payment
            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            builder.HasData(
                new Order 
                { 
                    OrderId = 1, 
                    CustomerId = 1, 
                    OrderDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                    TotalAmount = 2099.98m,
                    Status = OrderStatus.Delivered
                },
                new Order 
                { 
                    OrderId = 2, 
                    CustomerId = 2, 
                    OrderDate = new DateTime(2024, 2, 20, 14, 45, 0, DateTimeKind.Utc),
                    TotalAmount = 19.99m,
                    Status = OrderStatus.Shipped
                },
                new Order 
                { 
                    OrderId = 3, 
                    CustomerId = 1, 
                    OrderDate = new DateTime(2024, 3, 10, 9, 15, 0, DateTimeKind.Utc),
                    TotalAmount = 829.98m,
                    Status = OrderStatus.Processing
                },
                new Order 
                { 
                    OrderId = 4, 
                    CustomerId = 3, 
                    OrderDate = new DateTime(2024, 3, 25, 16, 20, 0, DateTimeKind.Utc),
                    TotalAmount = 29.99m,
                    Status = OrderStatus.Pending
                }
            );
        }
    }
}
