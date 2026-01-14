using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddTopCustomersView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW View_TopCustomersBySpending
                AS
                SELECT 
                    c.FullName AS CustomerName,
                    ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
                FROM 
                    Customers c
                LEFT JOIN 
                    Orders o ON c.CustomerId = o.CustomerId
                GROUP BY 
                    c.CustomerId, c.FullName
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS View_TopCustomersBySpending");
        }
    }
}
