using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.OrderItemId);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            // Relationship with Order
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Product
            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            builder.HasData(
                // Order 1: Smartphone + Laptop
                new OrderItem
                {
                    OrderItemId = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1
                },
                new OrderItem
                {
                    OrderItemId = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Quantity = 1
                },

                // Order 2: Fiction Book
                new OrderItem
                {
                    OrderItemId = 3,
                    OrderId = 2,
                    ProductId = 3,
                    Quantity = 1
                },

                // Order 3: Smartphone
                new OrderItem
                {
                    OrderItemId = 4,
                    OrderId = 3,
                    ProductId = 1,
                    Quantity = 1
                },

                // Order 4: T-Shirt
                new OrderItem
                {
                    OrderItemId = 5,
                    OrderId = 4,
                    ProductId = 4,
                    Quantity = 1
                }
            );
        }
    }
}
