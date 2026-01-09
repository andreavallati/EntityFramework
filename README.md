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

### 9. **Database Operations**
- Data seeding
- Migrations support (via EF Core Tools)
- Raw SQL execution for INSERT/UPDATE

### 10. **Database Views**
- TopCustomer view mapping
- Keyless entity types

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

   The application will:
   - Create the database if it doesn't exist
   - Seed initial data
   - Run demonstrations of all EF Core features

### Using Migrations (Optional)

Instead of `EnsureCreatedAsync()`, you can use migrations:

```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

---

## Resources

- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Performance](https://docs.microsoft.com/en-us/ef/core/performance/)
- [EF Core Best Practices](https://docs.microsoft.com/en-us/ef/core/miscellaneous/best-practices)

---

## Notes

- The project uses `EnsureCreatedAsync()` for simplicity. In production, use migrations.
- Connection string points to LocalDB by default. Update for other SQL Server instances.
- Logging is enabled for educational purposes. Disable sensitive data logging in production.
- Some demonstrations modify data. Re-run to see consistent behavior.

---

<div align="center">

**Happy Coding!**

</div>
