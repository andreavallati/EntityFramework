using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(200);

            // Owned entity for Address (JSON column in SQL Server)
            builder.OwnsOne(c => c.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Street).HasMaxLength(200);
                addressBuilder.Property(a => a.City).HasMaxLength(100);
                addressBuilder.Property(a => a.PostalCode).HasMaxLength(20);
            });
        }
    }
}
