using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Helpers
{
    public static class CompiledQueries
    {
        public static readonly Func<ECommerceContext, int, Task<Category?>> GetCategoryWithProductsAsync =
            EF.CompileAsyncQuery((ECommerceContext context, int categoryId) =>
                context.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == categoryId));

        public static readonly Func<ECommerceContext, int, Customer?> GetCustomerWithOrdersSync =
            EF.CompileQuery((ECommerceContext context, int customerId) =>
                context.Customers.Include(c => c.Orders).FirstOrDefault(c => c.CustomerId == customerId));
    }
}
