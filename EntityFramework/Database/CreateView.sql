-- SQL Script to create the View_TopCustomersBySpending view
-- This view is referenced in the TopCustomer entity configuration
-- Run this script manually after the database is created

USE ECommerceDb;
GO

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
GO

-- Sample query to test the view
-- SELECT * FROM View_TopCustomersBySpending ORDER BY TotalSpent DESC;
