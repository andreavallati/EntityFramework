using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary key
            builder.HasKey(p => p.ProductId);

            // Properties
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            // Concurrency token
            builder.Property(p => p.RowVersion)
                .IsRowVersion();

            // Unique index
            builder.HasIndex(p => p.Name)
                .IsUnique();

            // Query filter for soft delete
            builder.HasQueryFilter(p => !p.IsDeleted);

            // Relationship
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            builder.HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Smartphone",
                    Price = 799.99m,
                    CategoryId = 1,
                    IsDeleted = false,
                    RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Laptop",
                    Price = 1299.99m,
                    CategoryId = 1,
                    IsDeleted = false,
                    RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Fiction Book",
                    Price = 19.99m,
                    CategoryId = 2,
                    IsDeleted = false,
                    RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }
                },
                new Product
                {
                    ProductId = 4,
                    Name = "T-Shirt",
                    Price = 29.99m,
                    CategoryId = 3,
                    IsDeleted = false,
                    RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }
                }
            );
        }
    }
}
