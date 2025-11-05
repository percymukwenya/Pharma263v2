using System.Collections.Generic;
using System;

namespace Pharma263.MVC.Models
{
    #region Sales Report ViewModels
    public class MonthlySalesTrendViewModel
    {
        public DateTime Month { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public int UniqueCustomers { get; set; }
        public List<TopProductForMonth> TopProducts { get; set; }
    }

    public class TopProductForMonth
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class SalesRepPerformanceViewModel
    {
        public int SalesRepId { get; set; }
        public string SalesRepName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public int NewCustomersAcquired { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal TargetAchievementPercentage { get; set; }
        public List<MonthlySalesPerformance> MonthlyPerformance { get; set; }
    }

    public class MonthlySalesPerformance
    {
        public DateTime Month { get; set; }
        public decimal Sales { get; set; }
        public decimal Target { get; set; }
    }

    #endregion

    #region Inventory Report ViewModels

    public class ABCAnalysisViewModel
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal AnnualUsageValue { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public decimal CumulativePercentage { get; set; }
        public string Category { get; set; } // A, B, or C
        public int CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
    }

    public class InventoryAgingViewModel
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int CurrentStock { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public int DaysInInventory { get; set; }
        public string AgingBucket { get; set; } // 0-30, 31-60, 61-90, >90 days
        public DateTime LastPurchaseDate { get; set; }
        public DateTime LastSaleDate { get; set; }
    }

    public class ExpiryTrackingViewModel
    {
        public string ProductName { get; set; }
        public string BatchNumber { get; set; }
        public int CurrentStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysToExpiry { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string ExpiryStatus { get; set; } // Critical, Warning, Safe
    }

    public class StockTurnoverViewModel
    {
        public string ProductName { get; set; }
        public decimal TurnoverRatio { get; set; }
        public int AverageInventory { get; set; }
        public int TotalUnitsSold { get; set; }
        public int DaysOfInventory { get; set; }
        public decimal StockEfficiency { get; set; }
    }

    #endregion

    #region Financial Report ViewModels

    public class ProfitMarginViewModel
    {
        public string ProductCategory { get; set; }
        public decimal GrossSales { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossProfitMargin { get; set; }
        public decimal OperatingExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public decimal NetProfitMargin { get; set; }
        public List<MonthlyProfitTrend> MonthlyTrends { get; set; }
    }

    public class MonthlyProfitTrend
    {
        public DateTime Month { get; set; }
        public decimal GrossProfitMargin { get; set; }
        public decimal NetProfitMargin { get; set; }
    }

    public class ExpenseAnalysisViewModel
    {
        public string ExpenseCategory { get; set; }
        public decimal Amount { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public decimal MonthOverMonthChange { get; set; }
        public decimal YearOverYearChange { get; set; }
        public List<MonthlyExpense> MonthlyTrend { get; set; }
    }

    public class MonthlyExpense
    {
        public DateTime Month { get; set; }
        public decimal Amount { get; set; }
    }

    #endregion

    #region Customer Analysis ViewModels

    public class CustomerLifetimeValueViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalOrders { get; set; }
        public DateTime FirstPurchaseDate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public decimal CustomerLifetimeValue { get; set; }
        public string CustomerSegment { get; set; }
        public List<YearlyValue> YearlyTrends { get; set; }
    }

    public class YearlyValue
    {
        public int Year { get; set; }
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
    }

    public class CustomerRetentionViewModel
    {
        public DateTime CohortMonth { get; set; }
        public int InitialCustomerCount { get; set; }
        public List<MonthlyRetention> RetentionRates { get; set; }
        public decimal CustomerChurnRate { get; set; }
        public decimal RepeatPurchaseRate { get; set; }
        public decimal CustomerRetentionCost { get; set; }
    }

    public class MonthlyRetention
    {
        public int MonthNumber { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal RetentionRate { get; set; }
    }

    public class CustomerSegmentationViewModel
    {
        public string SegmentName { get; set; }
        public int CustomerCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal PurchaseFrequency { get; set; }
        public List<string> TopProducts { get; set; }
        public List<CustomerBehavior> BehaviorMetrics { get; set; }
    }

    public class CustomerBehavior
    {
        public string MetricName { get; set; }
        public decimal Value { get; set; }
    }

    #endregion

    #region Supplier Analysis ViewModels

    public class SupplierPerformanceViewModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public int TotalOrders { get; set; }
        public decimal OnTimeDeliveryRate { get; set; }
        public decimal QualityRating { get; set; }
        public decimal AverageLeadTime { get; set; }
        public decimal PriceCompetitiveness { get; set; }
        public List<MonthlyPerformance> MonthlyMetrics { get; set; }
        
        // Additional properties for dashboard compatibility
        public decimal PerformanceScore { get; set; }
        public decimal CostEfficiency { get; set; }
        public string RiskLevel { get; set; }
        public int TotalPurchases { get; set; }
    }

    public class MonthlyPerformance
    {
        public DateTime Month { get; set; }
        public decimal PurchaseValue { get; set; }
        public int OrderCount { get; set; }
        public decimal DeliveryRate { get; set; }
    }

    public class PurchaseTrendsViewModel
    {
        public DateTime Month { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<CategoryPurchase> CategoryBreakdown { get; set; }
        public List<SupplierShare> SupplierDistribution { get; set; }
    }

    public class InventoryTurnoverModel
    {
        public string ProductName { get; set; }
        public decimal TurnoverRatio { get; set; }
        public int AverageInventory { get; set; }
        public int TotalUnitsSold { get; set; }
        public int DaysOfInventory { get; set; }
        public string StockEfficiency { get; set; }
    }

    public class CategoryPurchase
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class SupplierShare
    {
        public string SupplierName { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal SharePercentage { get; set; }
    }

    #endregion
}
