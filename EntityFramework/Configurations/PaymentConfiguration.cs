using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2);

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            // Table-Per-Hierarchy inheritance
            builder.UseTphMappingStrategy()
                .HasDiscriminator<string>("PaymentType")
                .HasValue<CreditCardPayment>("CreditCard")
                .HasValue<PayPalPayment>("PayPal");

            // Configure discriminator column
            builder.Property("PaymentType")
                .HasMaxLength(50);

            // Seed data for different payment types
            builder.HasData(
                new
                {
                    PaymentId = 1,
                    OrderId = 1,
                    Amount = 2099.98m,
                    PaymentDate = new DateTime(2024, 1, 15, 10, 35, 0, DateTimeKind.Utc),
                    PaymentType = "CreditCard",
                    CardNumber = "4532-1234-5678-9010"
                },
                new
                {
                    PaymentId = 2,
                    OrderId = 2,
                    Amount = 19.99m,
                    PaymentDate = new DateTime(2024, 2, 20, 14, 50, 0, DateTimeKind.Utc),
                    PaymentType = "PayPal",
                    PayPalEmail = "jane.doe@example.com"
                },
                new
                {
                    PaymentId = 3,
                    OrderId = 3,
                    Amount = 829.98m,
                    PaymentDate = new DateTime(2024, 3, 10, 9, 20, 0, DateTimeKind.Utc),
                    PaymentType = "CreditCard",
                    CardNumber = "5425-2334-3010-9903"
                }
            );
        }
    }
}
