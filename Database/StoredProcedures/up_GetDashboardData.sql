-- =============================================
-- Author:		Claude Code Assistant  
-- Create date: 2025-01-18
-- Description:	Get current dashboard data with counts and low stock items
-- =============================================
CREATE OR ALTER PROCEDURE [Pharma263].[up_GetDashboardData]
    @LowStockAmount INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    -- First result set: Current dashboard counts
    SELECT 
        COUNT(DISTINCT c.Id) AS TotalCustomers,
        COUNT(DISTINCT m.Id) AS TotalMedicines,
        COUNT(DISTINCT si.Id) AS TotalStockItems,
        ISNULL(SUM(si.Quantity), 0) AS TotalStockQuantity,
        COUNT(DISTINCT s.Id) AS TotalSuppliers,
        COUNT(DISTINCT p.Id) AS TotalPurchases,
        ISNULL(SUM(p.TotalAmount), 0) AS TotalPurchaseAmount,
        COUNT(DISTINCT sal.Id) AS TotalSales,
        ISNULL(SUM(sal.TotalAmount), 0) AS TotalSalesAmount
    FROM 
        -- Adjust table names according to your actual schema
        Customer c
    CROSS JOIN Medicine m
    CROSS JOIN StockItem si
    CROSS JOIN Supplier s
    CROSS JOIN Purchase p
    CROSS JOIN Sale sal
    WHERE 
        c.IsActive = 1
        AND m.IsActive = 1
        AND si.IsActive = 1
        AND s.IsActive = 1
        AND p.IsActive = 1
        AND sal.IsActive = 1;

    -- Second result set: Low stock items
    SELECT 
        si.Id,
        m.Name AS MedicineName,
        si.BatchNo,
        si.Quantity AS TotalQuantity,
        si.BuyingPrice,
        si.SellingPrice
    FROM StockItem si
    INNER JOIN Medicine m ON si.MedicineId = m.Id
    WHERE 
        si.Quantity <= @LowStockAmount
        AND si.ExpiryDate > GETDATE()
        AND si.IsActive = 1
        AND m.IsActive = 1
    ORDER BY si.Quantity ASC, m.Name;

END