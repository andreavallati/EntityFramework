using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayPalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            // Seed Categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Books" },
                    { 3, "Clothing" }
                });

            // Seed Customers with JSON Address using raw SQL
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT Customers ON;
                
                INSERT INTO Customers (CustomerId, FullName, Address)
                VALUES 
                    (1, 'John Smith', '{""Street"":""123 Main St"",""City"":""New York"",""PostalCode"":""10001""}'),
                    (2, 'Jane Doe', '{""Street"":""456 Elm St"",""City"":""Los Angeles"",""PostalCode"":""90001""}'),
                    (3, 'Bob Wilson', '{""Street"":""789 Oak Ave"",""City"":""Chicago"",""PostalCode"":""60601""}');
                
                SET IDENTITY_INSERT Customers OFF;
            ");

            // Seed Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "IsDeleted", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, false, "Smartphone", 799.99m },
                    { 2, 1, false, "Laptop", 1299.99m },
                    { 3, 2, false, "Fiction Book", 19.99m },
                    { 4, 3, false, "T-Shirt", 29.99m }
                });

            // Seed Orders
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc), "Delivered", 2099.98m },
                    { 2, 2, new DateTime(2024, 2, 20, 14, 45, 0, DateTimeKind.Utc), "Shipped", 19.99m },
                    { 3, 1, new DateTime(2024, 3, 10, 9, 15, 0, DateTimeKind.Utc), "Processing", 829.98m },
                    { 4, 3, new DateTime(2024, 3, 25, 16, 20, 0, DateTimeKind.Utc), "Pending", 29.99m }
                });

            // Seed OrderItems
            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 2, 1 },
                    { 3, 2, 3, 1 },
                    { 4, 3, 1, 1 },
                    { 5, 4, 4, 1 }
                });

            // Seed Payments (TPH with all properties)
            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "OrderId", "PaymentDate", "PaymentType", "CardNumber", "PayPalEmail" },
                values: new object[,]
                {
                    { 1, 2099.98m, 1, new DateTime(2024, 1, 15, 10, 35, 0, DateTimeKind.Utc), "CreditCard", "4532-1234-5678-9010", null },
                    { 2, 19.99m, 2, new DateTime(2024, 2, 20, 14, 50, 0, DateTimeKind.Utc), "PayPal", null, "jane.doe@example.com" },
                    { 3, 829.98m, 3, new DateTime(2024, 3, 10, 9, 20, 0, DateTimeKind.Utc), "CreditCard", "5425-2334-3010-9903", null }
                });

            // Create Indexes
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "OrderItems");
            migrationBuilder.DropTable(name: "Payments");
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "Customers");
        }
    }
}
