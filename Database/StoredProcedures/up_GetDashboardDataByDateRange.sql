-- =============================================
-- Author:		Claude Code Assistant
-- Create date: 2025-01-18
-- Description:	Get dashboard data filtered by date range for trend analysis
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
    -- Note: Adjust table and column names according to your actual database schema
    
    -- For entities that existed during the date range (customers, suppliers, medicines created)
    DECLARE @CustomersInRange INT = (SELECT COUNT(*) FROM Customer WHERE CreatedDate <= @EndDate AND IsActive = 1)
    DECLARE @SuppliersInRange INT = (SELECT COUNT(*) FROM Supplier WHERE CreatedDate <= @EndDate AND IsActive = 1) 
    DECLARE @MedicinesInRange INT = (SELECT COUNT(*) FROM Medicine WHERE CreatedDate <= @EndDate AND IsActive = 1)
    
    -- For stock items that existed during the range
    DECLARE @StockItemsInRange INT = (SELECT COUNT(*) FROM StockItem WHERE CreatedDate <= @EndDate AND IsActive = 1)
    DECLARE @StockQuantityInRange INT = (SELECT ISNULL(SUM(Quantity), 0) FROM StockItem WHERE CreatedDate <= @EndDate AND IsActive = 1)
    
    -- For transactions within the date range
    DECLARE @PurchasesInRange INT = (SELECT COUNT(*) FROM Purchase WHERE PurchaseDate BETWEEN @StartDate AND @EndDate AND IsActive = 1)
    DECLARE @PurchaseAmountInRange DECIMAL(18,2) = (SELECT ISNULL(SUM(TotalAmount), 0) FROM Purchase WHERE PurchaseDate BETWEEN @StartDate AND @EndDate AND IsActive = 1)
    
    DECLARE @SalesInRange INT = (SELECT COUNT(*) FROM Sale WHERE SaleDate BETWEEN @StartDate AND @EndDate AND IsActive = 1)
    DECLARE @SalesAmountInRange DECIMAL(18,2) = (SELECT ISNULL(SUM(TotalAmount), 0) FROM Sale WHERE SaleDate BETWEEN @StartDate AND @EndDate AND IsActive = 1)

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

    -- Second result set: Low stock items (same as current procedure)
    SELECT 
        si.Id,
        m.Name AS MedicineName,
        si.BatchNo,
        si.Quantity AS TotalQuantity,
        si.BuyingPrice,
        si.SellingPrice
    FROM StockItem si
    INNER JOIN Medicine m ON si.MedicineId = m.Id
    WHERE si.Quantity <= @LowStockAmount
        AND si.ExpiryDate > GETDATE()
        AND si.IsActive = 1
    ORDER BY si.Quantity ASC, m.Name;

END