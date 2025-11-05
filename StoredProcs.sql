CREATE PROCEDURE [Pharma263].[GenerateAccountsPayableReport]
AS
BEGIN
    SELECT
        s.Id,
        s.[Name] AS SupplierName,
        ap.AmountOwed,
		ap.BalanceOwed,
        ap.DueDate
    FROM
        [Pharma263].Supplier s
    JOIN
        [Pharma263].AccountsPayable ap ON s.Id = ap.SupplierId
    WHERE
        ap.BalanceOwed > 0;
END
GO

CREATE PROCEDURE [Pharma263].[GenerateAccountsReceivableReport]
AS
BEGIN
    SELECT
        c.Id,
        c.[Name] AS CustomerName,
        ar.AmountDue,
		ar.BalanceDue,
        ar.DueDate
    FROM
        [Pharma263].Customer c
    JOIN
        [Pharma263].AccountsReceivable ar ON c.Id = ar.CustomerId
    WHERE
        ar.BalanceDue > 0;
END
GO


CREATE PROCEDURE [Pharma263].[GenerateSalesByProductReport]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
		m.[Name] AS MedicineName,
		SUM(s.Total) AS TotalSales,
		COUNT(s.Id) AS TotalTransactions
	FROM
		[Pharma263].Sales s
	JOIN [Pharma263].SalesItems si ON s.Id = si.SaleId
	JOIN [Pharma263].Stock st ON si.StockId = st.Id
	JOIN [Pharma263].Medicine m ON st.MedicineId = m.Id
	WHERE
		s.SalesDate BETWEEN @StartDate AND @EndDate
	GROUP BY
		m.[Name]
	ORDER BY m.[Name]
END
GO


CREATE PROCEDURE [Pharma263].[GenerateSalesSummaryReport]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
        SUM(s.Total) AS TotalRevenue,
        COUNT(s.Id) AS TotalTransactions
    FROM
        [Pharma263].Sales s
    WHERE
        s.SalesDate BETWEEN @StartDate AND @EndDate
END
GO


CREATE PROCEDURE [Pharma263].[GetCustomerPaymentReceivedHistory]
    @CustomerId INT
AS
BEGIN
    SELECT
        p.PaymentDate,
        p.AmountReceived,
		pm.[Name] AS PaymentMethod
    FROM
        [Pharma263].PaymentReceived p
		INNER JOIN [Pharma263].PaymentMethod pm ON pm.Id = p.PaymentMethodId
    WHERE
        p.AccountsReceivableId IN (SELECT ar.Id FROM [Pharma263].AccountsReceivable ar WHERE ar.CustomerId = @CustomerId)
    ORDER BY
        p.PaymentDate;
END
GO

CREATE PROCEDURE [Pharma263].[GetCustomersOwing]
AS
BEGIN
    SELECT
        c.Id,
        c.[Name] AS CustomerName,
        SUM(ar.BalanceDue) AS AmountOwing
    FROM
        [Pharma263].AccountsReceivable ar
		INNER JOIN [Pharma263].Customer c ON c.Id = ar.CustomerId
    GROUP BY
        c.Id, c.[Name]
    HAVING
        SUM(ar.BalanceDue) > 0;
END
GO


CREATE PROCEDURE [Pharma263].[GetPurchaseByProduct]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
        m.[Name] AS MedicineName,
        SUM(p.GrandTotal) AS TotalPurchaseCost,
        Count(p.Id) AS TotalTransactions
    FROM
        [Pharma263].Purchase p
	JOIN 
		[Pharma263].PurchaseItems pp ON pp.PurchaseId = p.Id
	JOIN
		[Pharma263].Medicine m ON pp.MedicineId = m.Id
    WHERE
        p.PurchaseDate BETWEEN @StartDate AND @EndDate
    GROUP BY
        m.[Name]
	ORDER BY m.[Name]
END
GO

CREATE PROCEDURE [Pharma263].[GetPurchaseBySupplier]
    @SupplierId INT
AS
BEGIN
    SELECT    
        m.[Name] AS MedicineName,
		p.PurchaseDate,
		pp.Amount,
        pp.Quantity,
		pp.Price
    FROM
        [Pharma263].Purchase p
	JOIN
		[Pharma263].PurchaseItems pp ON p.Id = pp.PurchaseId
	JOIN
		[Pharma263].Medicine m ON pp.MedicineId = m.Id
    WHERE
        SupplierId = @SupplierId
	GROUP BY
		m.[Name],
		p.PurchaseDate,
		pp.Amount,
        pp.Quantity,
		pp.Price
END
GO


CREATE PROCEDURE [Pharma263].[GetPurchaseSummary]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT
        SUM(p.GrandTotal) AS TotalExpenditure,
		COUNT(p.Id) AS TotalTransactions
    FROM
        [Pharma263].Purchase p
    WHERE
        p.PurchaseDate BETWEEN @StartDate AND @EndDate
END
GO

CREATE PROCEDURE [Pharma263].[GetSalesByCustomer]
    @CustomerId INT
AS
BEGIN
    SELECT
		m.[Name] AS MedicineName,
        s.SalesDate,		
        si.Amount,
		si.Quantity,
		si.Price
    FROM
        [Pharma263].Sales s
	JOIN [Pharma263].SalesItems si ON s.Id = si.SaleId
	JOIN [Pharma263].Stock st ON si.StockId = st.Id
	JOIN [Pharma263].Medicine m ON st.MedicineId = m.Id
    WHERE
        s.CustomerId = @CustomerId
	GROUP BY
		m.[Name],
		s.SalesDate,		
        si.Amount,
		si.Quantity,
		si.Price
END
GO

CREATE PROCEDURE [Pharma263].[GetSupplierPaymentHistory]
    @SupplierId INT
AS
BEGIN
    SELECT
        p.PaymentDate,
        p.AmountPaid,
		pm.[Name] AS PaymentMethod,
		p.AccountPayableId,
		p.SaleId
    FROM
        [Pharma263].PaymentMade p
		INNER JOIN [Pharma263].PaymentMethod pm ON pm.Id = p.PaymentMethodId
    WHERE
        p.AccountPayableId IN (SELECT ap.Id FROM [Pharma263].AccountsPayable ap WHERE ap.SupplierId = @SupplierId)
    ORDER BY
        PaymentDate;
END
GO


CREATE PROCEDURE [Pharma263].[GetSuppliersOwed]
AS
BEGIN
    SELECT
        s.Id,
        s.[Name] AS SupplierName,
        SUM(ap.AmountOwed) AS AmountOwed
    FROM [Pharma263].AccountsPayable ap
	INNER JOIN [Pharma263].Supplier s ON s.Id = ap.SupplierId
    GROUP BY
        s.Id, s.[Name]
    HAVING
        SUM(ap.BalanceOwed) > 0;
END
GO


CREATE PROCEDURE [Pharma263].[sp_ExpiryTracking]
    @MonthsToExpiry INT = 6
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        m.Name AS ProductName,
        s.BatchNo,
        s.TotalQuantity AS CurrentStock,
        s.ExpiryDate,
        DATEDIFF(DAY, GETDATE(), s.ExpiryDate) AS DaysToExpiry,
        s.BuyingPrice AS UnitCost,
        (s.TotalQuantity * s.BuyingPrice) AS TotalValue,
        CASE 
            WHEN DATEDIFF(DAY, GETDATE(), s.ExpiryDate) <= 30 THEN 'Critical'
            WHEN DATEDIFF(DAY, GETDATE(), s.ExpiryDate) <= 90 THEN 'Warning'
            ELSE 'Safe'
        END AS ExpiryStatus
    FROM 
        [Pharma263].[Stock] s
        INNER JOIN [Pharma263].[Medicine] m ON s.MedicineId = m.Id
    WHERE 
        s.TotalQuantity > 0
        AND DATEDIFF(MONTH, GETDATE(), s.ExpiryDate) <= @MonthsToExpiry
        AND s.IsDeleted = 0
        AND m.IsDeleted = 0
    ORDER BY 
        DaysToExpiry ASC;
END
GO


CREATE PROCEDURE [Pharma263].[up_ABCAnalysis]
    @AsOfDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH ProductValue AS (
        SELECT 
            m.Id,
            m.Name AS ProductName,
            m.GenericName,
            SUM(si.Amount) AS AnnualUsageValue,
            s.TotalQuantity AS CurrentStock,
            s.NotifyForQuantityBelow AS ReorderPoint
        FROM 
            [Pharma263].[Medicine] m
            LEFT JOIN [Pharma263].[Stock] s ON m.Id = s.MedicineId
            LEFT JOIN [Pharma263].[SalesItems] si ON s.Id = si.StockId
            LEFT JOIN [Pharma263].[Sales] sa ON si.SaleId = sa.Id
        WHERE 
            sa.SalesDate >= DATEADD(YEAR, -1, @AsOfDate)
            AND sa.SalesDate <= @AsOfDate
            AND m.IsDeleted = 0
            AND s.IsDeleted = 0
            AND si.IsDeleted = 0
            AND sa.IsDeleted = 0
        GROUP BY 
            m.Id, m.Name, m.GenericName, s.TotalQuantity, s.NotifyForQuantityBelow
    ),
    ValueAnalysis AS (
        SELECT 
            *,
            SUM(AnnualUsageValue) OVER () AS TotalValue,
            SUM(AnnualUsageValue) OVER (ORDER BY AnnualUsageValue DESC) / 
            NULLIF(SUM(AnnualUsageValue) OVER (), 0) * 100 AS CumulativePercentage
        FROM 
            ProductValue
    )
    SELECT 
        ProductName,
        GenericName,
        AnnualUsageValue,
        (AnnualUsageValue * 100.0 / NULLIF(TotalValue, 0)) AS PercentageOfTotal,
        CumulativePercentage,
        CASE 
            WHEN CumulativePercentage <= 80 THEN 'A'
            WHEN CumulativePercentage <= 95 THEN 'B'
            ELSE 'C'
        END AS Category,
        CurrentStock,
        ReorderPoint
    FROM 
        ValueAnalysis
    ORDER BY 
        AnnualUsageValue DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_CashFlowAnalysis]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH CashInflow AS (
        -- Cash from Sales and Payments Received
        SELECT 
            DATEFROMPARTS(YEAR(pr.PaymentDate), MONTH(pr.PaymentDate), 1) AS Month,
            SUM(pr.AmountReceived) AS CashFromSales,
            COUNT(DISTINCT pr.Id) AS NumberOfPaymentsReceived,
            COUNT(DISTINCT pr.CustomerId) AS NumberOfPayingCustomers
        FROM 
            [Pharma263].[PaymentReceived] pr
        WHERE 
            pr.PaymentDate BETWEEN @StartDate AND @EndDate
            AND pr.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(pr.PaymentDate), MONTH(pr.PaymentDate), 1)
    ),
    CashOutflow AS (
        -- Cash paid to Suppliers
        SELECT 
            DATEFROMPARTS(YEAR(pm.PaymentDate), MONTH(pm.PaymentDate), 1) AS Month,
            SUM(pm.AmountPaid) AS SupplierPayments,
            COUNT(DISTINCT pm.SupplierId) AS NumberOfSuppliersPaid,
            COUNT(DISTINCT pm.Id) AS NumberOfPaymentsMade
        FROM 
            [Pharma263].[PaymentMade] pm
        WHERE 
            pm.PaymentDate BETWEEN @StartDate AND @EndDate
            AND pm.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(pm.PaymentDate), MONTH(pm.PaymentDate), 1)
    ),
    AccountsReceivable AS (
        -- Current Accounts Receivable
        SELECT
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) AS Month,
            SUM(s.GrandTotal) AS TotalReceivables,
            COUNT(DISTINCT s.CustomerId) AS NumberOfCustomersOwing
        FROM
            [Pharma263].[Sales] s
        WHERE
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.PaymentMethodId != 1  -- Assuming 1 is cash payment
            AND s.IsDeleted = 0
        GROUP BY
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1)
    ),
    AccountsPayable AS (
        -- Current Accounts Payable
        SELECT
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1) AS Month,
            SUM(p.GrandTotal) AS TotalPayables,
            COUNT(DISTINCT p.SupplierId) AS NumberOfSuppliersOwed
        FROM
            [Pharma263].[Purchase] p
        WHERE
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.PaymentMethodId != 1  -- Assuming 1 is cash payment
            AND p.IsDeleted = 0
        GROUP BY
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1)
    )
    SELECT 
        COALESCE(ci.Month, co.Month, ar.Month, ap.Month) AS Month,
        -- Cash Inflow
        COALESCE(ci.CashFromSales, 0) AS CashInflow,
        COALESCE(ci.NumberOfPaymentsReceived, 0) AS NumberOfPaymentsReceived,
        COALESCE(ci.NumberOfPayingCustomers, 0) AS NumberOfPayingCustomers,
        
        -- Cash Outflow
        COALESCE(co.SupplierPayments, 0) AS CashOutflow,
        COALESCE(co.NumberOfSuppliersPaid, 0) AS NumberOfSuppliersPaid,
        COALESCE(co.NumberOfPaymentsMade, 0) AS NumberOfPaymentsMade,
        
        -- Net Cash Flow
        COALESCE(ci.CashFromSales, 0) - COALESCE(co.SupplierPayments, 0) AS NetCashFlow,
        
        -- Accounts Receivable and Payable
        COALESCE(ar.TotalReceivables, 0) AS AccountsReceivable,
        COALESCE(ar.NumberOfCustomersOwing, 0) AS NumberOfCustomersOwing,
        COALESCE(ap.TotalPayables, 0) AS AccountsPayable,
        COALESCE(ap.NumberOfSuppliersOwed, 0) AS NumberOfSuppliersOwed,
        
        -- Working Capital
        COALESCE(ar.TotalReceivables, 0) - COALESCE(ap.TotalPayables, 0) AS WorkingCapital
    FROM 
        CashInflow ci
        FULL OUTER JOIN CashOutflow co ON ci.Month = co.Month
        FULL OUTER JOIN AccountsReceivable ar ON ci.Month = ar.Month
        FULL OUTER JOIN AccountsPayable ap ON ci.Month = ap.Month
    ORDER BY 
        Month;
END
GO


CREATE PROCEDURE [Pharma263].[up_CustomerLifetimeValue]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH CustomerPurchases AS (
        SELECT 
            c.Id AS CustomerId,
            c.Name AS CustomerName,
            ct.Name AS CustomerType,
            MIN(s.SalesDate) AS FirstPurchaseDate,
            MAX(s.SalesDate) AS LastPurchaseDate,
            COUNT(DISTINCT s.Id) AS TotalOrders,
            SUM(s.GrandTotal) AS TotalRevenue,
            SUM(s.GrandTotal - s.Discount) AS NetRevenue,
            AVG(s.GrandTotal) AS AverageOrderValue
        FROM 
            [Pharma263].[Customer] c
            INNER JOIN [Pharma263].[CustomerType] ct ON c.CustomerTypeId = ct.Id
            LEFT JOIN [Pharma263].[Sales] s ON c.Id = s.CustomerId
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND c.IsDeleted = 0
            AND ct.IsDeleted = 0
            AND s.IsDeleted = 0
        GROUP BY 
            c.Id, c.Name, ct.Name
    ),
    Returns AS (
        -- Calculate returns for each customer
        SELECT 
            c.Id AS CustomerId,
            COUNT(DISTINCT r.Id) AS TotalReturns,
            SUM(si.Amount * (r.Quantity * 1.0 / si.Quantity)) AS TotalReturnAmount
        FROM 
            [Pharma263].[Customer] c
            JOIN [Pharma263].[Sales] s ON c.Id = s.CustomerId
            JOIN [Pharma263].[SalesItems] si ON s.Id = si.SaleId
            JOIN [Pharma263].[Returns] r ON s.Id = r.SaleId AND si.StockId = r.StockId
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND c.IsDeleted = 0
            AND s.IsDeleted = 0
            AND r.IsDeleted = 0
        GROUP BY 
            c.Id
    )
    SELECT 
        cp.CustomerId,
        cp.CustomerName,
        cp.CustomerType,
        cp.FirstPurchaseDate,
        cp.LastPurchaseDate,
        cp.TotalOrders,
        cp.TotalRevenue,
        cp.NetRevenue,
        cp.AverageOrderValue,
        SUM(s.Discount) AS TotalDiscounts,
        ISNULL(r.TotalReturns, 0) AS TotalReturns,
        ISNULL(r.TotalReturnAmount, 0) AS TotalReturnAmount,
        DATEDIFF(MONTH, cp.FirstPurchaseDate, cp.LastPurchaseDate) AS MonthsActive,
        CAST(cp.TotalOrders AS DECIMAL(10,2)) / 
            NULLIF(DATEDIFF(MONTH, cp.FirstPurchaseDate, cp.LastPurchaseDate), 0) AS OrdersPerMonth,
        cp.NetRevenue - ISNULL(r.TotalReturnAmount, 0) AS CustomerLifetimeValue,
        CASE 
            WHEN cp.NetRevenue - ISNULL(r.TotalReturnAmount, 0) >= 10000 THEN 'High Value'
            WHEN cp.NetRevenue - ISNULL(r.TotalReturnAmount, 0) >= 5000 THEN 'Medium Value'
            ELSE 'Low Value'
        END AS CustomerSegment,
        CASE 
            WHEN DATEDIFF(DAY, cp.LastPurchaseDate, GETDATE()) <= 90 THEN 'Active'
            WHEN DATEDIFF(DAY, cp.LastPurchaseDate, GETDATE()) <= 180 THEN 'At Risk'
            ELSE 'Inactive'
        END AS CustomerStatus
    FROM 
        CustomerPurchases cp
        LEFT JOIN Returns r ON cp.CustomerId = r.CustomerId
        LEFT JOIN [Pharma263].[Sales] s ON cp.CustomerId = s.CustomerId
    WHERE 
        s.SalesDate BETWEEN @StartDate AND @EndDate
    GROUP BY 
        cp.CustomerId, cp.CustomerName, cp.CustomerType, cp.FirstPurchaseDate, 
        cp.LastPurchaseDate, cp.TotalOrders, cp.TotalRevenue, cp.NetRevenue, 
        cp.AverageOrderValue, r.TotalReturns, r.TotalReturnAmount
    ORDER BY 
        CustomerLifetimeValue DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_CustomerRetention]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH CustomerCohorts AS (
        -- Get first purchase date for each customer
        SELECT 
            c.Id AS CustomerId,
            c.Name AS CustomerName,
            ct.Name AS CustomerType,
            DATEFROMPARTS(YEAR(MIN(s.SalesDate)), MONTH(MIN(s.SalesDate)), 1) AS CohortMonth
        FROM 
            [Pharma263].[Customer] c
            INNER JOIN [Pharma263].[CustomerType] ct ON c.CustomerTypeId = ct.Id
            INNER JOIN [Pharma263].[Sales] s ON c.Id = s.CustomerId
        WHERE 
            c.IsDeleted = 0
            AND ct.IsDeleted = 0
            AND s.IsDeleted = 0
        GROUP BY 
            c.Id, c.Name, ct.Name
    ),
    MonthlyActivity AS (
        -- Track customer activity by month
        SELECT 
            c.CustomerId,
            c.CohortMonth,
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) AS ActivityMonth,
            SUM(s.GrandTotal) AS MonthlySpend,
            SUM(s.Discount) AS MonthlyDiscounts
        FROM 
            CustomerCohorts c
            INNER JOIN [Pharma263].[Sales] s ON c.CustomerId = s.CustomerId
        WHERE 
            s.IsDeleted = 0
        GROUP BY 
            c.CustomerId,
            c.CohortMonth,
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1)
    )
    SELECT 
        c.CohortMonth,
        COUNT(DISTINCT c.CustomerId) AS InitialCustomerCount,
        COUNT(DISTINCT ma.CustomerId) AS ActiveCustomers,
        CAST(
            COUNT(DISTINCT ma.CustomerId) * 100.0 / 
            NULLIF(COUNT(DISTINCT c.CustomerId), 0) AS DECIMAL(5,2)
        ) AS RetentionRate,
        -- Churn Rate
        CAST(
            (COUNT(DISTINCT c.CustomerId) - COUNT(DISTINCT ma.CustomerId)) * 100.0 / 
            NULLIF(COUNT(DISTINCT c.CustomerId), 0) AS DECIMAL(5,2)
        ) AS CustomerChurnRate,
        -- Repeat Purchase Rate
        CAST(
            (SELECT COUNT(DISTINCT s2.CustomerId) 
             FROM [Pharma263].[Sales] s2 
             WHERE s2.CustomerId IN (
                 SELECT CustomerId FROM CustomerCohorts WHERE CohortMonth = c.CohortMonth
             )
             AND s2.IsDeleted = 0
             GROUP BY s2.CustomerId
             HAVING COUNT(*) > 1) * 100.0 / 
            NULLIF(COUNT(DISTINCT c.CustomerId), 0) AS DECIMAL(5,2)
        ) AS RepeatPurchaseRate,
        -- Average retention cost (using discounts as proxy)
        AVG(ma.MonthlyDiscounts) AS CustomerRetentionCost
    FROM 
        CustomerCohorts c
        LEFT JOIN MonthlyActivity ma ON 
            c.CustomerId = ma.CustomerId AND
            ma.ActivityMonth BETWEEN @StartDate AND @EndDate
    GROUP BY 
        c.CohortMonth
    ORDER BY 
        c.CohortMonth;
END
GO


CREATE PROCEDURE [Pharma263].[up_CustomerSegmentation]
    @AsOfDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH CustomerMetrics AS (
        SELECT 
            c.Id AS CustomerId,
            c.Name AS CustomerName,
            ct.Name AS CustomerTypeName,
            COUNT(DISTINCT s.Id) AS PurchaseFrequency,
            SUM(s.GrandTotal) AS TotalRevenue,
            AVG(s.GrandTotal) AS AverageOrderValue,
            MAX(s.SalesDate) AS LastPurchaseDate,
            DATEDIFF(DAY, MAX(s.SalesDate), @AsOfDate) AS DaysSinceLastPurchase
        FROM 
            [Pharma263].[Customer] c
            INNER JOIN [Pharma263].[CustomerType] ct ON c.CustomerTypeId = ct.Id
            LEFT JOIN [Pharma263].[Sales] s ON c.Id = s.CustomerId
        WHERE 
            c.IsDeleted = 0
            AND ct.IsDeleted = 0
            AND (s.IsDeleted = 0 OR s.IsDeleted IS NULL)
        GROUP BY 
            c.Id, c.Name, ct.Name
    ),
    TopProductsByCustomer AS (
        SELECT 
            s.CustomerId,
            m.Name AS ProductName,
            COUNT(*) AS PurchaseCount,
            ROW_NUMBER() OVER (PARTITION BY s.CustomerId ORDER BY COUNT(*) DESC) AS Rank
        FROM 
            [Pharma263].[Sales] s
            INNER JOIN [Pharma263].[SalesItems] si ON s.Id = si.SaleId
            INNER JOIN [Pharma263].[Stock] st ON si.StockId = st.Id
            INNER JOIN [Pharma263].[Medicine] m ON st.MedicineId = m.Id
        WHERE 
            s.IsDeleted = 0
            AND si.IsDeleted = 0
        GROUP BY 
            s.CustomerId, m.Name
    )
    SELECT 
        CASE 
            WHEN cm.TotalRevenue >= 10000 AND cm.PurchaseFrequency >= 12 THEN 'Premium'
            WHEN cm.TotalRevenue >= 5000 AND cm.PurchaseFrequency >= 6 THEN 'High Value'
            WHEN cm.TotalRevenue >= 1000 OR cm.PurchaseFrequency >= 3 THEN 'Medium Value'
            ELSE 'Low Value'
        END AS SegmentName,
        COUNT(DISTINCT cm.CustomerId) AS CustomerCount,
        SUM(cm.TotalRevenue) AS TotalRevenue,
        AVG(cm.AverageOrderValue) AS AverageOrderValue,
        AVG(cm.PurchaseFrequency) AS AveragePurchaseFrequency,
        STRING_AGG(CASE WHEN tp.Rank = 1 THEN tp.ProductName END, ', ') AS TopProducts,
        AVG(cm.DaysSinceLastPurchase) AS AverageDaysSinceLastPurchase,
        COUNT(CASE WHEN cm.DaysSinceLastPurchase <= 90 THEN 1 END) AS ActiveCustomers,
        COUNT(CASE WHEN cm.DaysSinceLastPurchase > 90 AND cm.DaysSinceLastPurchase <= 180 THEN 1 END) AS AtRiskCustomers,
        COUNT(CASE WHEN cm.DaysSinceLastPurchase > 180 THEN 1 END) AS ChurnedCustomers
    FROM 
        CustomerMetrics cm
        LEFT JOIN TopProductsByCustomer tp ON cm.CustomerId = tp.CustomerId
    GROUP BY 
        CASE 
            WHEN cm.TotalRevenue >= 10000 AND cm.PurchaseFrequency >= 12 THEN 'Premium'
            WHEN cm.TotalRevenue >= 5000 AND cm.PurchaseFrequency >= 6 THEN 'High Value'
            WHEN cm.TotalRevenue >= 1000 OR cm.PurchaseFrequency >= 3 THEN 'Medium Value'
            ELSE 'Low Value'
        END
    ORDER BY 
        TotalRevenue DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_ExpiryTracking]
    @MonthsToExpiry INT = 6
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        m.Name AS ProductName,
        s.BatchNo,
        s.TotalQuantity AS CurrentStock,
        s.ExpiryDate,
        DATEDIFF(DAY, GETDATE(), s.ExpiryDate) AS DaysToExpiry,
        s.BuyingPrice AS UnitCost,
        (s.TotalQuantity * s.BuyingPrice) AS TotalValue,
        CASE 
            WHEN DATEDIFF(DAY, GETDATE(), s.ExpiryDate) <= 30 THEN 'Critical'
            WHEN DATEDIFF(DAY, GETDATE(), s.ExpiryDate) <= 90 THEN 'Warning'
            ELSE 'Safe'
        END AS ExpiryStatus
    FROM 
        [Pharma263].[Stock] s
        INNER JOIN [Pharma263].[Medicine] m ON s.MedicineId = m.Id
    WHERE 
        s.TotalQuantity > 0
        AND DATEDIFF(MONTH, GETDATE(), s.ExpiryDate) <= @MonthsToExpiry
        AND s.IsDeleted = 0
        AND m.IsDeleted = 0
    ORDER BY 
        DaysToExpiry ASC;
END
GO


CREATE PROCEDURE [Pharma263].[up_GetDashboardData]
	@LowStockAmount INT
AS
BEGIN
		DECLARE @TotalCustomers INT,
				@TotalMedicines INT,
				@TotalStockItems INT,
				@TotalStockQuantity INT,
				@TotalSuppliers INT,
				@TotalPurchases INT,
				@TotalPurchaseAmount DECIMAL(18, 2),
				@TotalSales INT,
				@TotalSalesAmount DECIMAL(18, 2)

		SELECT @TotalCustomers = COUNT(*) FROM [Pharma263].Customer c WITH (NOLOCK) WHERE c.IsDeleted = 0

		SELECT @TotalMedicines = COUNT(*) FROM [Pharma263].Medicine m WITH (NOLOCK) WHERE m.IsDeleted = 0

		SELECT 
			@TotalStockItems = COUNT(s.Id), 
			@TotalStockQuantity = SUM(s.TotalQuantity) 
		FROM [Pharma263].[Stock] s WITH (NOLOCK)

		SELECT @TotalSuppliers = COUNT(*) FROM [Pharma263].Supplier s WITH (NOLOCK) WHERE s.IsDeleted = 0

		SELECT 
			@TotalPurchases = COUNT(p.Id), 
			@TotalPurchaseAmount = SUM(p.GrandTotal) 
		FROM [Pharma263].[Purchase] p WITH (NOLOCK)
		WHERE p.IsDeleted = 0

		SELECT 
			@TotalSales = COUNT(s.Id), 
			@TotalSalesAmount = SUM(s.GrandTotal) 
		FROM [Pharma263].[Sales] s WITH (NOLOCK)
		WHERE s.IsDeleted = 0

		SELECT 
			@TotalCustomers AS TotalCustomers,
			@TotalMedicines AS TotalMedicines,
			@TotalStockItems AS TotalStockItems,
			@TotalStockQuantity AS TotalStockQuantity,
			@TotalSuppliers AS TotalSuppliers,
			@TotalPurchases AS TotalPurchases,
			@TotalPurchaseAmount AS TotalPurchaseAmount,
			@TotalSales AS TotalSales,
			@TotalSalesAmount AS TotalSalesAmount

		SELECT 
				m.[Name], 
				s.BatchNo, 
				s.TotalQuantity, 
				s.SellingPrice, 
				s.BuyingPrice 
		FROM [Pharma263].[Stock] s WITH (NOLOCK)
		INNER JOIN [Pharma263].[PurchaseItems] p WITH (NOLOCK) ON p.BatchNo = s.BatchNo AND p.MedicineId = s.MedicineId
		INNER JOIN [Pharma263].Medicine m WITH (NOLOCK) ON m.Id = p.MedicineId
		WHERE s.TotalQuantity <= @LowStockAmount AND s.IsDeleted = 0
		ORDER BY m.[Name]
END
GO


CREATE PROCEDURE [Pharma263].[up_InventoryAging]
    @AsOfDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH LastSale AS (
        -- Get last sale date for each stock item
        SELECT 
            si.StockId,
            MAX(s.SalesDate) AS LastSaleDate
        FROM 
            [Pharma263].[SalesItems] si
            INNER JOIN [Pharma263].[Sales] s ON si.SaleId = s.Id
        WHERE 
            si.IsDeleted = 0 AND s.IsDeleted = 0
        GROUP BY 
            si.StockId
    )
    SELECT 
        m.Name AS ProductName,
        m.GenericName,
        st.BatchNo,
        st.TotalQuantity AS CurrentStock,
        st.BuyingPrice AS UnitCost,
        st.TotalQuantity * st.BuyingPrice AS TotalValue,
        st.ExpiryDate,
        DATEDIFF(DAY, st.CreatedDate, @AsOfDate) AS DaysInInventory,
        CASE 
            WHEN DATEDIFF(DAY, st.CreatedDate, @AsOfDate) <= 30 THEN '0-30 Days'
            WHEN DATEDIFF(DAY, st.CreatedDate, @AsOfDate) <= 60 THEN '31-60 Days'
            WHEN DATEDIFF(DAY, st.CreatedDate, @AsOfDate) <= 90 THEN '61-90 Days'
            ELSE 'Over 90 Days'
        END AS AgingBucket,
        st.CreatedDate AS StockCreatedDate,
        ls.LastSaleDate,
        DATEDIFF(DAY, ls.LastSaleDate, @AsOfDate) AS DaysSinceLastSale,
        CASE 
            WHEN st.TotalQuantity > st.NotifyForQuantityBelow * 2 THEN 'Overstocked'
            WHEN st.TotalQuantity > st.NotifyForQuantityBelow THEN 'Adequate'
            WHEN st.TotalQuantity <= st.NotifyForQuantityBelow THEN 'Low Stock'
            WHEN st.TotalQuantity = 0 THEN 'Out of Stock'
        END AS StockStatus,
        CASE 
            WHEN DATEDIFF(DAY, ls.LastSaleDate, @AsOfDate) > 90 THEN 'Slow Moving'
            WHEN DATEDIFF(DAY, ls.LastSaleDate, @AsOfDate) > 180 THEN 'Non-Moving'
            ELSE 'Active'
        END AS MovementStatus
    FROM 
        [Pharma263].[Stock] st
        INNER JOIN [Pharma263].[Medicine] m ON st.MedicineId = m.Id
        LEFT JOIN LastSale ls ON st.Id = ls.StockId
    WHERE 
        st.IsDeleted = 0 
        AND m.IsDeleted = 0
    ORDER BY 
        DaysInInventory DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_MonthlySalesTrends]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH MonthlySales AS (
        SELECT 
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) AS Month,
            SUM(s.GrandTotal) AS TotalSales,
            SUM(s.Discount) AS TotalDiscounts,
            COUNT(DISTINCT s.Id) AS TotalTransactions,
            COUNT(DISTINCT s.CustomerId) AS UniqueCustomers,
            LAG(SUM(s.GrandTotal)) OVER (ORDER BY DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1)) AS PreviousMonthSales
        FROM 
            [Pharma263].[Sales] s
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1)
    ),
    TopProducts AS (
        SELECT 
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) AS Month,
            m.Name AS ProductName,
            SUM(si.Quantity) AS QuantitySold,
            SUM(si.Amount) AS Revenue,
            ROW_NUMBER() OVER (PARTITION BY DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) 
                              ORDER BY SUM(si.Amount) DESC) AS RankByRevenue
        FROM 
            [Pharma263].[Sales] s
            INNER JOIN [Pharma263].[SalesItems] si ON s.Id = si.SaleId
            INNER JOIN [Pharma263].[Stock] st ON si.StockId = st.Id
            INNER JOIN [Pharma263].[Medicine] m ON st.MedicineId = m.Id
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.IsDeleted = 0
            AND si.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1),
            m.Name
    )
    SELECT 
        ms.Month,
        ms.TotalSales,
        ms.TotalDiscounts,
        ms.TotalTransactions,
        ms.TotalSales / NULLIF(ms.TotalTransactions, 0) AS AverageTransactionValue,
        CASE 
            WHEN ms.PreviousMonthSales IS NULL OR ms.PreviousMonthSales = 0 THEN 0
            ELSE ((ms.TotalSales - ms.PreviousMonthSales) * 100.0 / ms.PreviousMonthSales)
        END AS GrowthPercentage,
        ms.UniqueCustomers,
        tp.ProductName AS TopProduct,
        tp.QuantitySold AS TopProductQuantity,
        tp.Revenue AS TopProductRevenue
    FROM 
        MonthlySales ms
        LEFT JOIN TopProducts tp ON ms.Month = tp.Month AND tp.RankByRevenue = 1
    ORDER BY 
        ms.Month;
END
GO

CREATE PROCEDURE [Pharma263].[up_ProfitMarginAnalysis]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH SalesData AS (
        SELECT 
            m.Name AS ProductCategory, -- Using Medicine Name as category since there's no category field
            s.SalesDate,
            si.Amount AS GrossSales,
            (st.BuyingPrice * si.Quantity) AS CostOfGoods,
            si.Amount - (st.BuyingPrice * si.Quantity) AS GrossProfit,
            si.Quantity
        FROM 
            [Pharma263].[Sales] s
            INNER JOIN [Pharma263].[SalesItems] si ON s.Id = si.SaleId
            INNER JOIN [Pharma263].[Stock] st ON si.StockId = st.Id
            INNER JOIN [Pharma263].[Medicine] m ON st.MedicineId = m.Id
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.IsDeleted = 0
            AND si.IsDeleted = 0
            AND st.IsDeleted = 0
            AND m.IsDeleted = 0
    )
    SELECT 
        ProductCategory,
        SUM(GrossSales) AS GrossSales,
        SUM(CostOfGoods) AS TotalCosts,
        SUM(GrossProfit) AS GrossProfit,
        CAST(
            (SUM(GrossProfit) * 100.0 / NULLIF(SUM(GrossSales), 0)) AS DECIMAL(5,2)
        ) AS GrossProfitMargin,
        -- Estimated Operating Expenses (15% of Gross Sales as placeholder)
        SUM(GrossSales) * 0.15 AS EstimatedOperatingExpenses,
        -- Net Profit
        SUM(GrossProfit) - (SUM(GrossSales) * 0.15) AS NetProfit,
        -- Net Profit Margin
        CAST(
            ((SUM(GrossProfit) - (SUM(GrossSales) * 0.15)) * 100.0 / 
            NULLIF(SUM(GrossSales), 0)) AS DECIMAL(5,2)
        ) AS NetProfitMargin
    FROM 
        SalesData
    GROUP BY 
        ProductCategory
    ORDER BY 
        GrossProfit DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_PurchaseTrends]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH MonthlyPurchases AS (
        SELECT 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1) AS PurchaseMonth,
            SUM(p.GrandTotal) AS TotalPurchaseAmount,
            COUNT(DISTINCT p.Id) AS TotalOrders,
            COUNT(DISTINCT p.SupplierId) AS UniqueSuppliers
        FROM 
            [Pharma263].[Purchase] p
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1)
    ),
    MedicineBreakdown AS (
        SELECT 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1) AS PurchaseMonth,
            m.Name AS MedicineName,
            SUM(pi.Amount) AS CategoryAmount,
            COUNT(DISTINCT p.Id) AS CategoryOrders
        FROM 
            [Pharma263].[Purchase] p
            INNER JOIN [Pharma263].[PurchaseItems] pi ON p.Id = pi.PurchaseId
            INNER JOIN [Pharma263].[Medicine] m ON pi.MedicineId = m.Id
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
            AND pi.IsDeleted = 0
            AND m.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1),
            m.Name
    ),
    SupplierAnalysis AS (
        SELECT 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1) AS PurchaseMonth,
            s.Name AS SupplierName,
            SUM(p.GrandTotal) AS SupplierAmount
        FROM 
            [Pharma263].[Purchase] p
            INNER JOIN [Pharma263].[Supplier] s ON p.SupplierId = s.Id
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
            AND s.IsDeleted = 0
        GROUP BY 
            DATEFROMPARTS(YEAR(p.PurchaseDate), MONTH(p.PurchaseDate), 1),
            s.Name
    )
    SELECT 
        mp.PurchaseMonth,
        mp.TotalPurchaseAmount,
        mp.TotalOrders,
        mp.TotalPurchaseAmount / NULLIF(mp.TotalOrders, 0) AS AverageOrderValue,
        mp.UniqueSuppliers,
        mb.MedicineName,
        mb.CategoryAmount,
        CAST(mb.CategoryAmount * 100.0 / NULLIF(mp.TotalPurchaseAmount, 0) AS DECIMAL(5,2)) AS CategoryPercentage,
        sa.SupplierName,
        sa.SupplierAmount,
        CAST(sa.SupplierAmount * 100.0 / NULLIF(mp.TotalPurchaseAmount, 0) AS DECIMAL(5,2)) AS SupplierPercentage
    FROM 
        MonthlyPurchases mp
        LEFT JOIN MedicineBreakdown mb ON mp.PurchaseMonth = mb.PurchaseMonth
        LEFT JOIN SupplierAnalysis sa ON mp.PurchaseMonth = sa.PurchaseMonth
    ORDER BY 
        mp.PurchaseMonth DESC, CategoryPercentage DESC;
END
GO

CREATE PROCEDURE [Pharma263].[up_SalesRepPerformance]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Get first purchase date for each customer
    WITH FirstPurchases AS (
        SELECT 
            CustomerId,
            MIN(SalesDate) AS FirstPurchaseDate
        FROM 
            [Pharma263].[Sales]
        WHERE 
            IsDeleted = 0
        GROUP BY 
            CustomerId
    ),
    -- Calculate monthly sales for each rep
    MonthlyStats AS (
        SELECT 
            s.CreatedBy AS SalesRepId,
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1) AS Month,
            COUNT(DISTINCT s.Id) AS MonthlyTransactions,
            SUM(s.GrandTotal) AS MonthlySales,
            COUNT(DISTINCT s.CustomerId) AS MonthlyCustomers,
            COUNT(DISTINCT CASE 
                WHEN fp.FirstPurchaseDate = s.SalesDate 
                THEN s.CustomerId 
                END) AS NewCustomers
        FROM 
            [Pharma263].[Sales] s
            LEFT JOIN FirstPurchases fp ON s.CustomerId = fp.CustomerId
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.IsDeleted = 0
        GROUP BY 
            s.CreatedBy,
            DATEFROMPARTS(YEAR(s.SalesDate), MONTH(s.SalesDate), 1)
    )
    -- Calculate final metrics
    SELECT 
        ms.SalesRepId,
        SUM(ms.MonthlySales) AS TotalSales,
        SUM(ms.MonthlyTransactions) AS TotalTransactions,
        CASE 
            WHEN SUM(ms.MonthlyTransactions) = 0 THEN 0 
            ELSE SUM(ms.MonthlySales) / SUM(ms.MonthlyTransactions) 
        END AS AverageTransactionValue,
        SUM(ms.NewCustomers) AS NewCustomersAcquired,
        AVG(ms.MonthlySales) AS AverageMonthlyRevenue,
        COUNT(DISTINCT DATEADD(MONTH, DATEDIFF(MONTH, 0, Month), 0)) AS MonthsActive,
        COUNT(DISTINCT ms.MonthlyCustomers) AS TotalCustomers
    FROM 
        MonthlyStats ms
    GROUP BY 
        ms.SalesRepId
    ORDER BY 
        TotalSales DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_StockTurnover]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH InventorySnapshots AS (
        -- Calculate average inventory based on stock records
        SELECT 
            m.Id AS MedicineId,
            m.Name AS ProductName,
            m.GenericName,
            AVG(CAST(s.TotalQuantity AS FLOAT)) AS AverageInventory,
            MAX(s.TotalQuantity) AS MaxInventory,
            MIN(s.TotalQuantity) AS MinInventory,
            s.NotifyForQuantityBelow AS ReorderPoint,
            AVG(s.BuyingPrice) AS AverageCost
        FROM 
            [Pharma263].[Medicine] m
            LEFT JOIN [Pharma263].[Stock] s ON m.Id = s.MedicineId
        WHERE 
            m.IsDeleted = 0
            AND s.IsDeleted = 0
        GROUP BY 
            m.Id, m.Name, m.GenericName, s.NotifyForQuantityBelow
    ),
    SalesData AS (
        -- Calculate total units sold
        SELECT 
            st.MedicineId,
            SUM(si.Quantity) AS TotalUnitsSold,
            SUM(si.Amount) AS TotalSalesValue
        FROM 
            [Pharma263].[Stock] st
            JOIN [Pharma263].[SalesItems] si ON st.Id = si.StockId
            JOIN [Pharma263].[Sales] s ON si.SaleId = s.Id
        WHERE 
            s.SalesDate BETWEEN @StartDate AND @EndDate
            AND s.IsDeleted = 0
            AND si.IsDeleted = 0
            AND st.IsDeleted = 0
        GROUP BY 
            st.MedicineId
    ),
    LastSale AS (
        -- Get last sale date for each product
        SELECT 
            st.MedicineId,
            MAX(s.SalesDate) AS LastSaleDate
        FROM 
            [Pharma263].[Stock] st
            JOIN [Pharma263].[SalesItems] si ON st.Id = si.StockId
            JOIN [Pharma263].[Sales] s ON si.SaleId = s.Id
        WHERE 
            s.IsDeleted = 0
            AND si.IsDeleted = 0
            AND st.IsDeleted = 0
        GROUP BY 
            st.MedicineId
    )
    SELECT 
        i.ProductName,
        i.GenericName,
        CAST(COALESCE(sd.TotalUnitsSold, 0) / NULLIF(i.AverageInventory, 0) AS DECIMAL(10,2)) AS TurnoverRatio,
        i.AverageInventory,
        i.MaxInventory,
        i.MinInventory,
        i.ReorderPoint,
        COALESCE(sd.TotalUnitsSold, 0) AS TotalUnitsSold,
        COALESCE(sd.TotalSalesValue, 0) AS TotalSalesValue,
        i.AverageCost,
        CASE 
            WHEN sd.TotalUnitsSold > 0 
            THEN CAST(365.0 / NULLIF(sd.TotalUnitsSold / NULLIF(i.AverageInventory, 0), 0) AS INT)
            ELSE 0 
        END AS DaysOfInventory,
        ls.LastSaleDate,
        DATEDIFF(DAY, ls.LastSaleDate, GETDATE()) AS DaysSinceLastSale,
        CASE 
            WHEN COALESCE(sd.TotalUnitsSold, 0) / NULLIF(i.AverageInventory, 0) >= 12 THEN 'High'
            WHEN COALESCE(sd.TotalUnitsSold, 0) / NULLIF(i.AverageInventory, 0) >= 6 THEN 'Medium'
            ELSE 'Low'
        END AS StockEfficiency,
        CASE
            WHEN DATEDIFF(DAY, ls.LastSaleDate, GETDATE()) > 90 THEN 'Slow Moving'
            WHEN DATEDIFF(DAY, ls.LastSaleDate, GETDATE()) > 180 THEN 'Non-Moving'
            ELSE 'Active'
        END AS InventoryStatus
    FROM 
        InventorySnapshots i
        LEFT JOIN SalesData sd ON i.MedicineId = sd.MedicineId
        LEFT JOIN LastSale ls ON i.MedicineId = ls.MedicineId
    ORDER BY 
        TurnoverRatio DESC;
END
GO


CREATE PROCEDURE [Pharma263].[up_SupplierPerformance]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH SupplierPurchases AS (
        SELECT 
            s.Id AS SupplierId,
            s.Name AS SupplierName,
            COUNT(DISTINCT p.Id) AS TotalOrders,
            SUM(p.GrandTotal) AS TotalPurchaseValue,
            AVG(p.GrandTotal) AS AverageOrderValue,
            COUNT(DISTINCT pi.MedicineId) AS ProductVarietyCount,
            MAX(p.PurchaseDate) AS LastOrderDate
        FROM 
            [Pharma263].[Suppliers] s
            INNER JOIN [Pharma263].[Purchase] p ON s.Id = p.SupplierId
            INNER JOIN [Pharma263].[PurchaseItem] pi ON p.Id = pi.PurchaseId
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
            AND s.IsDeleted = 0
        GROUP BY 
            s.Id, s.Name
    ),
    DeliveryPerformance AS (
        SELECT 
            p.SupplierId,
            COUNT(CASE WHEN p.DeliveryDate <= p.ExpectedDeliveryDate THEN 1 END) AS OnTimeDeliveries,
            COUNT(*) AS TotalDeliveries,
            AVG(DATEDIFF(DAY, p.PurchaseDate, COALESCE(p.DeliveryDate, GETDATE()))) AS AverageLeadTimeDays
        FROM 
            [Pharma263].[Purchase] p
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
            AND p.ExpectedDeliveryDate IS NOT NULL
        GROUP BY 
            p.SupplierId
    ),
    PaymentCompliance AS (
        SELECT 
            pm.SupplierId,
            COUNT(CASE WHEN pm.PaymentDate <= pm.DueDate THEN 1 END) AS OnTimePayments,
            COUNT(*) AS TotalPayments
        FROM 
            [Pharma263].[PaymentMade] pm
        WHERE 
            pm.PaymentDate BETWEEN @StartDate AND @EndDate
            AND pm.IsDeleted = 0
        GROUP BY 
            pm.SupplierId
    )
    SELECT 
        sp.SupplierId,
        sp.SupplierName,
        sp.TotalPurchaseValue,
        sp.TotalOrders,
        sp.AverageOrderValue,
        CASE 
            WHEN dp.TotalDeliveries > 0 
            THEN CAST((dp.OnTimeDeliveries * 100.0 / dp.TotalDeliveries) AS DECIMAL(5,2))
            ELSE 0 
        END AS OnTimeDeliveryRate,
        COALESCE(dp.OnTimeDeliveries, 0) AS OnTimeDeliveries,
        COALESCE(dp.TotalDeliveries, 0) AS TotalDeliveries,
        -- Quality rating based on returns and complaints (simplified to 4.5 for now)
        4.5 AS QualityRating,
        COALESCE(dp.AverageLeadTimeDays, 0) AS AverageLeadTimeDays,
        -- Price competitiveness (simplified calculation)
        85.0 AS PriceCompetitiveness,
        sp.ProductVarietyCount,
        CASE 
            WHEN pc.TotalPayments > 0 
            THEN CAST((pc.OnTimePayments * 100.0 / pc.TotalPayments) AS DECIMAL(5,2))
            ELSE 100 
        END AS PaymentTermsCompliance,
        sp.LastOrderDate,
        CASE 
            WHEN sp.TotalPurchaseValue >= 100000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 95 THEN 'A'
            WHEN sp.TotalPurchaseValue >= 50000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 85 THEN 'B'
            WHEN sp.TotalPurchaseValue >= 10000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 75 THEN 'C'
            ELSE 'D'
        END AS PerformanceGrade,
        CASE 
            WHEN sp.TotalPurchaseValue >= 100000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 95 THEN 'Strategic Partner'
            WHEN sp.TotalPurchaseValue >= 50000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 85 THEN 'Preferred Supplier'
            WHEN sp.TotalPurchaseValue >= 10000 AND dp.OnTimeDeliveries * 100.0 / NULLIF(dp.TotalDeliveries, 0) >= 75 THEN 'Approved Supplier'
            ELSE 'Review Required'
        END AS RecommendedAction
    FROM 
        SupplierPurchases sp
        LEFT JOIN DeliveryPerformance dp ON sp.SupplierId = dp.SupplierId
        LEFT JOIN PaymentCompliance pc ON sp.SupplierId = pc.SupplierId
    ORDER BY 
        sp.TotalPurchaseValue DESC;
END
GO

CREATE PROCEDURE [Pharma263].[up_ExpenseAnalysis]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate period for comparison (same length previous period)
    DECLARE @PeriodDays INT = DATEDIFF(DAY, @StartDate, @EndDate);
    DECLARE @PrevStartDate DATE = DATEADD(DAY, -@PeriodDays - 1, @StartDate);
    DECLARE @PrevEndDate DATE = DATEADD(DAY, -1, @StartDate);

    WITH CurrentPeriodExpenses AS (
        SELECT 
            'Purchase Costs' AS ExpenseCategory,
            SUM(p.GrandTotal) AS CurrentPeriodAmount
        FROM 
            [Pharma263].[Purchase] p
        WHERE 
            p.PurchaseDate BETWEEN @StartDate AND @EndDate
            AND p.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Payment Processing' AS ExpenseCategory,
            SUM(pm.Amount) AS CurrentPeriodAmount
        FROM 
            [Pharma263].[PaymentMade] pm
        WHERE 
            pm.PaymentDate BETWEEN @StartDate AND @EndDate
            AND pm.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Inventory Adjustments' AS ExpenseCategory,
            SUM(CASE WHEN si.AdjustmentType = 'Loss' THEN si.Quantity * si.UnitCost ELSE 0 END) AS CurrentPeriodAmount
        FROM 
            [Pharma263].[StockAdjustment] sa
            INNER JOIN [Pharma263].[StockAdjustmentItem] si ON sa.Id = si.StockAdjustmentId
        WHERE 
            sa.AdjustmentDate BETWEEN @StartDate AND @EndDate
            AND sa.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Returns & Refunds' AS ExpenseCategory,
            SUM(r.RefundAmount) AS CurrentPeriodAmount
        FROM 
            [Pharma263].[Returns] r
        WHERE 
            r.DateReturned BETWEEN @StartDate AND @EndDate
            AND r.IsDeleted = 0
    ),
    PreviousPeriodExpenses AS (
        SELECT 
            'Purchase Costs' AS ExpenseCategory,
            SUM(p.GrandTotal) AS PreviousPeriodAmount
        FROM 
            [Pharma263].[Purchase] p
        WHERE 
            p.PurchaseDate BETWEEN @PrevStartDate AND @PrevEndDate
            AND p.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Payment Processing' AS ExpenseCategory,
            SUM(pm.Amount) AS PreviousPeriodAmount
        FROM 
            [Pharma263].[PaymentMade] pm
        WHERE 
            pm.PaymentDate BETWEEN @PrevStartDate AND @PrevEndDate
            AND pm.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Inventory Adjustments' AS ExpenseCategory,
            SUM(CASE WHEN si.AdjustmentType = 'Loss' THEN si.Quantity * si.UnitCost ELSE 0 END) AS PreviousPeriodAmount
        FROM 
            [Pharma263].[StockAdjustment] sa
            INNER JOIN [Pharma263].[StockAdjustmentItem] si ON sa.Id = si.StockAdjustmentId
        WHERE 
            sa.AdjustmentDate BETWEEN @PrevStartDate AND @PrevEndDate
            AND sa.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Returns & Refunds' AS ExpenseCategory,
            SUM(r.RefundAmount) AS PreviousPeriodAmount
        FROM 
            [Pharma263].[Returns] r
        WHERE 
            r.DateReturned BETWEEN @PrevStartDate AND @PrevEndDate
            AND r.IsDeleted = 0
    ),
    YearToDateExpenses AS (
        SELECT 
            'Purchase Costs' AS ExpenseCategory,
            SUM(p.GrandTotal) AS YearToDateAmount
        FROM 
            [Pharma263].[Purchase] p
        WHERE 
            p.PurchaseDate BETWEEN DATEFROMPARTS(YEAR(@EndDate), 1, 1) AND @EndDate
            AND p.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Payment Processing' AS ExpenseCategory,
            SUM(pm.Amount) AS YearToDateAmount
        FROM 
            [Pharma263].[PaymentMade] pm
        WHERE 
            pm.PaymentDate BETWEEN DATEFROMPARTS(YEAR(@EndDate), 1, 1) AND @EndDate
            AND pm.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Inventory Adjustments' AS ExpenseCategory,
            SUM(CASE WHEN si.AdjustmentType = 'Loss' THEN si.Quantity * si.UnitCost ELSE 0 END) AS YearToDateAmount
        FROM 
            [Pharma263].[StockAdjustment] sa
            INNER JOIN [Pharma263].[StockAdjustmentItem] si ON sa.Id = si.StockAdjustmentId
        WHERE 
            sa.AdjustmentDate BETWEEN DATEFROMPARTS(YEAR(@EndDate), 1, 1) AND @EndDate
            AND sa.IsDeleted = 0
        
        UNION ALL
        
        SELECT 
            'Returns & Refunds' AS ExpenseCategory,
            SUM(r.RefundAmount) AS YearToDateAmount
        FROM 
            [Pharma263].[Returns] r
        WHERE 
            r.DateReturned BETWEEN DATEFROMPARTS(YEAR(@EndDate), 1, 1) AND @EndDate
            AND r.IsDeleted = 0
    )
    SELECT 
        COALESCE(cpe.ExpenseCategory, ppe.ExpenseCategory, yte.ExpenseCategory) AS ExpenseCategory,
        COALESCE(cpe.CurrentPeriodAmount, 0) AS CurrentPeriodAmount,
        COALESCE(ppe.PreviousPeriodAmount, 0) AS PreviousPeriodAmount,
        COALESCE(cpe.CurrentPeriodAmount, 0) - COALESCE(ppe.PreviousPeriodAmount, 0) AS VarianceAmount,
        CASE 
            WHEN COALESCE(ppe.PreviousPeriodAmount, 0) = 0 THEN 0
            ELSE ((COALESCE(cpe.CurrentPeriodAmount, 0) - COALESCE(ppe.PreviousPeriodAmount, 0)) * 100.0 / ppe.PreviousPeriodAmount)
        END AS VariancePercentage,
        -- Simplified budget amounts (would normally come from budget table)
        COALESCE(cpe.CurrentPeriodAmount, 0) * 1.1 AS BudgetAmount,
        COALESCE(cpe.CurrentPeriodAmount, 0) - (COALESCE(cpe.CurrentPeriodAmount, 0) * 1.1) AS BudgetVariance,
        CASE 
            WHEN COALESCE(cpe.CurrentPeriodAmount, 0) * 1.1 = 0 THEN 0
            ELSE ((COALESCE(cpe.CurrentPeriodAmount, 0) - (COALESCE(cpe.CurrentPeriodAmount, 0) * 1.1)) * 100.0 / (COALESCE(cpe.CurrentPeriodAmount, 0) * 1.1))
        END AS BudgetVariancePercentage,
        COALESCE(yte.YearToDateAmount, 0) AS YearToDateAmount,
        COALESCE(yte.YearToDateAmount, 0) / NULLIF(DATEDIFF(MONTH, DATEFROMPARTS(YEAR(@EndDate), 1, 1), @EndDate) + 1, 0) AS AverageMonthlyAmount,
        CASE 
            WHEN COALESCE(cpe.CurrentPeriodAmount, 0) > COALESCE(ppe.PreviousPeriodAmount, 0) THEN 'Increasing'
            WHEN COALESCE(cpe.CurrentPeriodAmount, 0) < COALESCE(ppe.PreviousPeriodAmount, 0) THEN 'Decreasing'
            ELSE 'Stable'
        END AS TrendDirection
    FROM 
        CurrentPeriodExpenses cpe
        FULL OUTER JOIN PreviousPeriodExpenses ppe ON cpe.ExpenseCategory = ppe.ExpenseCategory
        FULL OUTER JOIN YearToDateExpenses yte ON COALESCE(cpe.ExpenseCategory, ppe.ExpenseCategory) = yte.ExpenseCategory
    ORDER BY 
        COALESCE(cpe.CurrentPeriodAmount, 0) DESC;
END
GO