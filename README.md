# Entity Framework

## Overview
A comprehensive demonstration of **Entity Framework Core 9.0** features using a real-world e-commerce domain model. This project showcases best practices and advanced EF Core capabilities.

---

## Features
### 1. **DbContext Configuration**
- Connection string management via `appsettings.json`
- Logging and diagnostics configuration
- Sensitive data logging for development
- Configuration-based setup

### 2. **Entity Relationships**
- **One-to-Many**: Category -> Products, Customer -> Orders
- **Many-to-Many**: Order -> Products (via OrderItem join entity)
- **One-to-One**: Order -> Payment
- Cascade delete and restrict behaviors

### 3. **Entity Configuration Patterns**
- `IEntityTypeConfiguration<T>` for organized model configuration
- Fluent API usage in separate configuration classes
- Automatic configuration discovery with `ApplyConfigurationsFromAssembly`

### 4. **Advanced Features**

#### **Inheritance (Table-Per-Hierarchy)**
- Payment base class with CreditCardPayment and PayPalPayment subtypes
- Discriminator-based storage in a single table

#### **Owned Entities**
- Address as JSON column embedded in Customer
- Complex type configuration

#### **Value Converters**
- OrderStatus enum stored as string in database
- Custom conversion configuration

#### **Concurrency Control**
- RowVersion (timestamp) token on Product entity
- Optimistic concurrency demonstration
- Conflict detection and handling

#### **Query Filters (Soft Delete)**
- Global query filter on Product.IsDeleted
- Automatic filtering of soft-deleted records
- `IgnoreQueryFilters()` to access deleted items

### 5. **Performance Optimizations**

#### **Compiled Queries**
- Pre-compiled LINQ queries for frequently-used operations
- Both async and sync compiled query examples

#### **AsNoTracking**
- Read-only queries without change tracking overhead
- Performance improvement for reporting scenarios

#### **Split Queries**
- `AsSplitQuery()` to avoid cartesian explosion
- Separate SQL queries for related entities

### 6. **Query Capabilities**
- Complex LINQ queries with projections
- Grouping and aggregations
- Raw SQL queries with `FromSqlInterpolated`
- Eager loading with `Include` and `ThenInclude`
- Filtered includes

### 7. **Transactions**
- Explicit transaction management
- Commit and rollback scenarios
- Multiple operations within transaction scope

### 8. **Change Tracking**
- `ChangeTracker` inspection
- Modified entity detection
- Original vs. current value comparison

### 9. **Data Seeding**
- Declarative seeding with `HasData()` for most entities
- Raw SQL seeding for JSON column limitations
- TPH inheritance seeding with discriminator

### 10. **Database Views**
- TopCustomer view mapping
- Keyless entity types

---

## Domain Model

```
┌─────────────┐         ┌─────────────┐         ┌─────────────┐
│  Category   │────1:N──│   Product   │         │  Customer   │
│             │         │             │         │             │
│ CategoryId  │         │ ProductId   │         │ CustomerId  │
│ Name        │         │ Name        │         │ FullName    │
└─────────────┘         │ Price       │         │ Address ◄───JSON
                        │ IsDeleted   │         └──────┬──────┘
                        │ RowVersion  │                │
                        │ CategoryId  │                │1:N
                        └──────┬──────┘                │
                               │                       │
                            N:1│                       │
                               │                       │
┌─────────────┐         ┌─────▼──────┐         ┌──────▼──────┐
│  OrderItem  │────N:1──│   Order    │────N:1──│  Customer   │
│             │         │            │         └─────────────┘
│ OrderItemId │         │ OrderId    │
│ OrderId     │         │ OrderDate  │
│ ProductId   │         │ TotalAmount│
│ Quantity    │         │ Status     │
└─────────────┘         │ CustomerId │
                        └──────┬─────┘
                               │1:1
                               │
                        ┌──────▼──────┐
                        │  Payment    │◄─── TPH Base
                        │             │
                        │ PaymentId   │
                        │ Amount      │
                        │ PaymentDate │
                        │ OrderId     │
                        └─────┬───┬───┘
                              │   │
                    ┌─────────┘   └──────────┐
                    │                        │
          ┌─────────▼──────────┐   ┌─────────▼──────────┐
          │ CreditCardPayment  │   │  PayPalPayment     │
          │                    │   │                    │
          │ CardNumber         │   │ PayPalEmail        │
          └────────────────────┘   └────────────────────┘
```

---

## Project Structure

```
EntityFramework/
├── Context/
│   └── ECommerceContext.cs            # DbContext with configurations
│
├── Entities/
│   ├── Category.cs                     # Category entity
│   ├── Product.cs                      # Product entity with concurrency token
│   ├── Customer.cs                     # Customer entity
│   ├── Address.cs                      # Owned entity (JSON column)
│   ├── Order.cs                        # Order entity
│   ├── OrderItem.cs                    # OrderItem junction entity
│   ├── Payment.cs                      # Payment base class (TPH)
│   ├── CreditCardPayment.cs            # Credit card payment type
│   ├── PayPalPayment.cs                # PayPal payment type
│   └── TopCustomer.cs                  # View-mapped entity
│
├── Configurations/
│   ├── CategoryConfiguration.cs        # Fluent API for Category
│   ├── ProductConfiguration.cs         # Fluent API for Product
│   ├── CustomerConfiguration.cs        # Fluent API for Customer
│   ├── OrderConfiguration.cs           # Fluent API for Order
│   ├── OrderItemConfiguration.cs       # Fluent API for OrderItem
│   ├── PaymentConfiguration.cs         # Fluent API for Payment (TPH)
│   └── TopCustomerConfiguration.cs     # Fluent API for TopCustomer view
│
├── Helpers/
│   └── CompiledQueries.cs             # Pre-compiled query definitions
│
├── Enums/
│   └── OrderStatus.cs                  # Order status enumeration
│
├── Migrations/
│   ├── 20250101000000_InitialCreate.*       # Initial schema & seed data
│   ├── 20250102000000_AddTopCustomersView.* # Database view
│   └── ECommerceContextModelSnapshot.cs
│
├── Database/
│   └── CreateView.sql                  # Original SQL for view (reference)
│
└── appsettings.json                    # Configuration & connection string
└── EntityFramework.csproj              # Project file
└── Program.cs                          # Main demo application
```

---

## Database Schema

### Tables Created
- **Categories** - Product categories
- **Products** - Product catalog with soft delete and concurrency control
- **Customers** - Customer information with JSON address column
- **Orders** - Customer orders
- **OrderItems** - Order line items (junction table)
- **Payments** - Payment information (TPH inheritance with discriminator)

### Views Created
- **View_TopCustomersBySpending** - Aggregated customer spending report

### Seed Data
- 3 Categories (Electronics, Books, Clothing)
- 4 Products (Smartphone, Laptop, Fiction Book, T-Shirt)
- 3 Customers with JSON addresses
- 4 Orders with various statuses
- 5 Order items
- 3 Payments (Credit Card and PayPal types)

---

## Database Migrations

This project uses Entity Framework Core migrations to manage the database schema and seed data.

### 1. InitialCreate (20250101000000)
- Creates all database tables (Categories, Products, Customers, Orders, OrderItems, Payments)
- Applies all entity configurations (indexes, foreign keys, constraints)
- Seeds initial data for all entities
- Implements TPH (Table-Per-Hierarchy) inheritance for Payment types
- Uses raw SQL for Customer seeding due to JSON column limitations

### 2. AddTopCustomersView (20250102000000)
- Creates the `View_TopCustomersBySpending` database view
- Aggregates customer spending data for reporting

The application is configured to automatically apply pending migrations when it starts:

```csharp
await context.Database.MigrateAsync();
```

### First Time Setup
1. Ensure SQL Server LocalDB is installed
2. Run the application - it will automatically create the database and apply all migrations
3. The connection string is in `appsettings.json`

### Manual Migration Commands

If you have `dotnet ef` tools installed, you can use these commands:

```bash
# List all migrations
dotnet ef migrations list

# Apply migrations to the database
dotnet ef database update

# Rollback to a specific migration
dotnet ef database update InitialCreate

# Remove the database (careful!)
dotnet ef database drop

# Generate SQL script for migrations
dotnet ef migrations script
```

### Creating New Migrations

When you modify entity configurations or add new entities:

```bash
# Create a new migration
dotnet ef migrations add YourMigrationName

# Apply it to the database
dotnet ef database update
```

### Database View

The `View_TopCustomersBySpending` view calculates total spending per customer:

```sql
SELECT 
    c.FullName AS CustomerName,
    ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Customers c
LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
GROUP BY c.CustomerId, c.FullName
```

This view is used by the `TopCustomer` entity for read-only queries.

### Important Notes

- **Migrations vs EnsureCreated**: The project uses migrations. Don't use `EnsureCreated()` as it bypasses migration history.
- **Seed Data**: Seed data is in the migration file and entity configurations. Changes to seed data require new migrations.
- **View Creation**: Database views must be created using raw SQL in migrations (see `AddTopCustomersView`).
- **Concurrency**: Products have a `RowVersion` column for optimistic concurrency control.
- **JSON Columns**: The `Address` owned entity is stored as a JSON column. JSON-mapped entities don't support `HasData()` for seeding - seed data must be inserted using raw SQL in migrations (see `InitialCreate` migration).

---

## Technologies Used
- .NET Core
- Entity Framework Core
- SQL Server (LocalDB)

---

## Installation
### Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (or update connection string in `appsettings.json`)

### Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd EntityFramework
   ```

2. **Update connection string** (optional)
   ```bash
   Edit `appsettings.json` to match your SQL Server instance.
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

The application will automatically:
- Create the database
- Apply migrations
- Seed sample data
- Run 12 comprehensive demos

---

## Resources

- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Performance](https://docs.microsoft.com/en-us/ef/core/performance/)
- [EF Core Best Practices](https://docs.microsoft.com/en-us/ef/core/miscellaneous/best-practices)

---

<div align="center">

**Happy Coding!**

</div>
