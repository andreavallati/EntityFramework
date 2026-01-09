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
        }
    }
}
