using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations
{
    public class TopCustomerConfiguration : IEntityTypeConfiguration<TopCustomer>
    {
        public void Configure(EntityTypeBuilder<TopCustomer> builder)
        {
            builder.ToView("View_TopCustomersBySpending");
            builder.HasNoKey();
        }
    }
}
