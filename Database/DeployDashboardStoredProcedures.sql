-- =============================================
-- Dashboard Stored Procedures Deployment Script
-- Run this script to create the dashboard stored procedures
-- Based on actual Pharma263 database schema
-- =============================================

USE [Pharma263] -- Using actual database name from schema
GO

-- =============================================
-- Main Dashboard Data Procedure
-- =============================================
CREATE OR ALTER PROCEDURE [Pharma263].[up_GetDashboardData]
    @LowStockAmount INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- First result set: Current dashboard counts
    SELECT 
        (SELECT COUNT(*) FROM [Pharma263].[Customer] WHERE IsDeleted = 0) AS TotalCustomers,
        (SELECT COUNT(*) FROM [Pharma263].[Medicine] WHERE IsDeleted = 0) AS TotalMedicines,
        (SELECT COUNT(*) FROM [Pharma263].[Stock] WHERE IsDeleted = 0) AS TotalStockItems,
        (SELECT ISNULL(SUM(TotalQuantity), 0) FROM [Pharma263].[Stock] WHERE IsDeleted = 0) AS TotalStockQuantity,
        (SELECT COUNT(*) FROM [Pharma263].[Supplier] WHERE IsDeleted = 0) AS TotalSuppliers,
        (SELECT COUNT(*) FROM [Pharma263].[Purchase] WHERE IsDeleted = 0) AS TotalPurchases,
        (SELECT ISNULL(SUM(GrandTotal), 0) FROM [Pharma263].[Purchase] WHERE IsDeleted = 0) AS TotalPurchaseAmount,
        (SELECT COUNT(*) FROM [Pharma263].[Sales] WHERE IsDeleted = 0) AS TotalSales,
        (SELECT ISNULL(SUM(GrandTotal), 0) FROM [Pharma263].[Sales] WHERE IsDeleted = 0) AS TotalSalesAmount;

    -- Second result set: Low stock items
    SELECT 
        s.Id,
        m.Name AS MedicineName,
        s.BatchNo,
        s.TotalQuantity,
        s.BuyingPrice,
        s.SellingPrice
    FROM [Pharma263].[Stock] s
    INNER JOIN [Pharma263].[Medicine] m ON s.MedicineId = m.Id
    WHERE 
        s.TotalQuantity <= @LowStockAmount
        AND s.ExpiryDate > GETDATE()
        AND s.IsDeleted = 0
        AND m.IsDeleted = 0
    ORDER BY s.TotalQuantity ASC, m.Name;

END
GO

-- =============================================
-- Dashboard Data by Date Range Procedure (for trends)
-- =============================================
CREATE OR ALTER PROCEDURE [Pharma263].[up_GetDashboardDataByDateRange]
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @LowStockAmount INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Set default dates if not provided
    IF @StartDate IS NULL
        SET @StartDate = DATEADD(DAY, -30, GETDATE())
    
    IF @EndDate IS NULL
        SET @EndDate = GETDATE()

    -- First result set: Dashboard counts filtered by date range
    -- Using actual Pharma263 database schema
    
    -- For entities that existed during the date range (using CreatedDate)
    DECLARE @CustomersInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Customer] WHERE CreatedDate <= @EndDate AND IsDeleted = 0)
    DECLARE @SuppliersInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Supplier] WHERE CreatedDate <= @EndDate AND IsDeleted = 0) 
    DECLARE @MedicinesInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Medicine] WHERE CreatedDate <= @EndDate AND IsDeleted = 0)
    
    -- For stock items that existed during the range
    DECLARE @StockItemsInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Stock] WHERE CreatedDate <= @EndDate AND IsDeleted = 0)
    DECLARE @StockQuantityInRange INT = (SELECT ISNULL(SUM(TotalQuantity), 0) FROM [Pharma263].[Stock] WHERE CreatedDate <= @EndDate AND IsDeleted = 0)
    
    -- For transactions within the date range (using actual date fields and amounts)
    DECLARE @PurchasesInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Purchase] WHERE PurchaseDate BETWEEN @StartDate AND @EndDate AND IsDeleted = 0)
    DECLARE @PurchaseAmountInRange DECIMAL(18,2) = (SELECT ISNULL(SUM(GrandTotal), 0) FROM [Pharma263].[Purchase] WHERE PurchaseDate BETWEEN @StartDate AND @EndDate AND IsDeleted = 0)
    
    DECLARE @SalesInRange INT = (SELECT COUNT(*) FROM [Pharma263].[Sales] WHERE SalesDate BETWEEN @StartDate AND @EndDate AND IsDeleted = 0)
    DECLARE @SalesAmountInRange DECIMAL(18,2) = (SELECT ISNULL(SUM(GrandTotal), 0) FROM [Pharma263].[Sales] WHERE SalesDate BETWEEN @StartDate AND @EndDate AND IsDeleted = 0)

    -- Return the aggregated data
    SELECT 
        @CustomersInRange AS TotalCustomers,
        @MedicinesInRange AS TotalMedicines,
        @StockItemsInRange AS TotalStockItems,
        @StockQuantityInRange AS TotalStockQuantity,
        @SuppliersInRange AS TotalSuppliers,
        @PurchasesInRange AS TotalPurchases,
        @PurchaseAmountInRange AS TotalPurchaseAmount,
        @SalesInRange AS TotalSales,
        @SalesAmountInRange AS TotalSalesAmount;

    -- Second result set: Low stock items (same as main procedure)
    SELECT 
        s.Id,
        m.Name AS MedicineName,
        s.BatchNo,
        s.TotalQuantity,
        s.BuyingPrice,
        s.SellingPrice
    FROM [Pharma263].[Stock] s
    INNER JOIN [Pharma263].[Medicine] m ON s.MedicineId = m.Id
    WHERE 
        s.TotalQuantity <= @LowStockAmount
        AND s.ExpiryDate > GETDATE()
        AND s.IsDeleted = 0
        AND m.IsDeleted = 0
    ORDER BY s.TotalQuantity ASC, m.Name;

END
GO

-- =============================================
-- Verification Queries (optional - for testing)
-- =============================================

-- Test the main procedure
-- EXEC [Pharma263].[up_GetDashboardData] @LowStockAmount = 10

-- Test the date range procedure with last 30 days
-- EXEC [Pharma263].[up_GetDashboardDataByDateRange] 
--     @StartDate = NULL, 
--     @EndDate = NULL, 
--     @LowStockAmount = 10

-- Test the date range procedure with specific dates
-- EXEC [Pharma263].[up_GetDashboardDataByDateRange] 
--     @StartDate = '2024-12-01', 
--     @EndDate = '2024-12-31', 
--     @LowStockAmount = 10

-- Test for trends by comparing current vs previous month
-- DECLARE @CurrentMonth DATETIME = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
-- DECLARE @PreviousMonth DATETIME = DATEADD(MONTH, -1, @CurrentMonth)
-- EXEC [Pharma263].[up_GetDashboardDataByDateRange] 
--     @StartDate = @PreviousMonth, 
--     @EndDate = @CurrentMonth, 
--     @LowStockAmount = 10

PRINT 'Dashboard stored procedures created successfully!'
PRINT 'Schema used: [Pharma263] database with correct table names:'
PRINT '- Customer (IsDeleted = 0)'
PRINT '- Medicine (IsDeleted = 0)' 
PRINT '- Stock (IsDeleted = 0, TotalQuantity, ExpiryDate)'
PRINT '- Supplier (IsDeleted = 0)'
PRINT '- Purchase (IsDeleted = 0, PurchaseDate, GrandTotal)'
PRINT '- Sales (IsDeleted = 0, SalesDate, GrandTotal)'
PRINT ''
PRINT 'Key features:'
PRINT '- Soft delete support (IsDeleted = 0)'
PRINT '- Date range filtering for trends'
PRINT '- Low stock alerts with expiry date validation'
PRINT '- Uses GrandTotal for purchase/sales amounts'