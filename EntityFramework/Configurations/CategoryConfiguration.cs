using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Seed data
            builder.HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Electronics"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Books"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Clothing"
                }
            );
        }
    }
}
