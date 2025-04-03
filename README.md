# Entity Framework

## Overview
This project is an Entity Framework Core sample implementation of an e-commerce system. It demonstrates key EF Core features, such as:
- **DbContext Configuration**
- **One-to-Many and Many-to-Many Relationships**
- **Compiled Queries for Performance Optimization**
- **Transactions and Data Seeding**

## Features
- **Entity Relationships:** Customers, Orders, Products, Payments, etc.
- **EF Core Transactions:** Example usage in `Program.cs`.
- **Compiled Queries:** Optimized queries using `EF.CompileQuery`.

## Technologies Used
- .NET Core
- Entity Framework Core
- SQL Server (LocalDB)

## Project Structure
```
EntityFramework/
│── Context/
│   ├── ECommerceContext.cs                # Database context configuration
│
│── Entities/
│   ├── Address.cs                         # JSON Column Example: Embedded Address
│   ├── Category.cs                        # Product and Category (One-to-Many)
│   ├── CreditCardPayment.cs               # CreditCard payment type
│   ├── Customer.cs                        # Customer and Orders (One-to-Many)
│   ├── Order.cs                           # Many-to-Many relationship with Products
│   ├── OrderItem.cs                       # Many-to-Many (Order and Products via OrderItem)
│   ├── Payment.cs                         # Base class for Payment (TPH)
│   ├── PayPalPayment.cs                   # PayPal payment type
│   ├── Product.cs                         # Many-to-Many relationship with Order
│   ├── TopCustomer.cs                     # View Example: Top Customers by Spending
│
│── Helpers/
│   ├── CompiledQueries.cs                 # Performance-optimized queries
│
│── Program.cs                             # Entry point and demo usage
│── EntityFramework.csproj                 # Project file
```

## Installation
### Prerequisites
- .NET SDK 6.0 or later
- SQL Server (if using database authentication)

### Steps
1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd EntityFramework
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Modify `ECommerceContext.cs` to set the correct connection string for SQL Server:
   ```sh
   optionsBuilder.UseSqlServer("Server=(localdb)\\YourInstance;Database=YourDatabase;Trusted_Connection=True;");
   ```

## Usage
### 3. Run the Project
To execute the program:
   ```sh
   $ dotnet run
   ```
This will:
- Ensure the database is created.
- Seed data if necessary.
- Demonstrate compiled queries and transactions.
