using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models
{
    public class AccountsPayableReportModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public double AmountOwed { get; set; }
        public double AmountPaid { get; set; }
        public double BalanceOwed { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class MonthlySalesTrendModel
    {
        public DateTime Month { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public int UniqueCustomers { get; set; }
        public string TopProduct { get; set; }
        public int TopProductQuantity { get; set; }
        public decimal TopProductRevenue { get; set; }
    }

    public class StockSummaryModel
    {
        public string MedicineName { get; set; }
        public string ProductCode { get; set; }
        public int CurrentStock { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal UnitCost { get; set; }
        public decimal CurrentStockValue { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal PurchaseValue { get; set; }
        public int SalesQuantity { get; set; }
        public decimal SalesValue { get; set; }
        public int ReorderPoint { get; set; }
        public string StockStatus { get; set; }
        public int DaysToExpiry { get; set; }
        public string ExpiryStatus { get; set; }
    }

    public class InventoryAgingModel
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string BatchNumber { get; set; }
        public int CurrentStock { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string ExpiryDate { get; set; }
        public int DaysInInventory { get; set; }
        public string AgingBucket { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public DateTime? LastSaleDate { get; set; }
        public int DaysSinceLastSale { get; set; }
        public string StockStatus { get; set; }
        public string MovementStatus { get; set; }
    }

    public class CustomerLifetimeValueModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public DateTime FirstPurchaseDate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal NetRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal TotalDiscounts { get; set; }
        public int TotalReturns { get; set; }
        public decimal TotalReturnAmount { get; set; }
        public int MonthsActive { get; set; }
        public decimal OrdersPerMonth { get; set; }
        public decimal CustomerLifetimeValue { get; set; }
        public string CustomerSegment { get; set; }
        public string CustomerStatus { get; set; }
    }

    public class CustomerRetentionModel
    {
        public DateTime CohortMonth { get; set; }
        public int InitialCustomerCount { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal RetentionRate { get; set; }
        public decimal CustomerChurnRate { get; set; }
        public decimal RepeatPurchaseRate { get; set; }
        public decimal CustomerRetentionCost { get; set; }
    }

    public class ABCAnalysisModel
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal AnnualUsageValue { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public decimal CumulativePercentage { get; set; }
        public string Category { get; set; }
        public int CurrentStock { get; set; }
        public decimal ReorderPoint { get; set; }
    }

    public class ProfitMarginModel
    {
        public string ProductCategory { get; set; }
        public decimal GrossSales { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossProfitMargin { get; set; }
        public decimal OperatingExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public decimal NetProfitMargin { get; set; }
    }

    public class SalesRepPerformanceModel
    {
        public int SalesRepId { get; set; }
        public string SalesRepName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public int NewCustomersAcquired { get; set; }
        public decimal AverageMonthlyTarget { get; set; }
        public decimal TargetAchievementPercentage { get; set; }
        public int TotalCustomers { get; set; }
    }

    public class ExpiryTrackingModel
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

    public class InventoryTurnoverModel
    {
        public string ProductName { get; set; }
        public decimal TurnoverRatio { get; set; }
        public int AverageInventory { get; set; }
        public int TotalUnitsSold { get; set; }
        public int DaysOfInventory { get; set; }
        public string StockEfficiency { get; set; } // High, Medium, Low
    }

    public class CustomerSegmentationModel
    {
        public string SegmentName { get; set; }
        public int CustomerCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal PurchaseFrequency { get; set; }
        public List<string> TopProducts { get; set; }
        public List<CustomerBehaviorMetric> BehaviorMetrics { get; set; }
    }

    public class CustomerBehaviorMetric
    {
        public string MetricName { get; set; }
        public decimal Value { get; set; }
    }

    public class CashFlowAnalysisModel
    {
        public DateTime Month { get; set; }
        public decimal CashInflow { get; set; }
        public decimal SupplierPayments { get; set; }
        public decimal OperatingExpenses { get; set; }
        public decimal TotalCashOutflow { get; set; }
        public decimal NetCashFlow { get; set; }
    }

    public class SupplierPerformanceModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal OnTimeDeliveryRate { get; set; }
        public int OnTimeDeliveries { get; set; }
        public int TotalDeliveries { get; set; }
        public decimal QualityRating { get; set; }
        public int AverageLeadTimeDays { get; set; }
        public decimal PriceCompetitiveness { get; set; }
        public int ProductVarietyCount { get; set; }
        public decimal PaymentTermsCompliance { get; set; }
        public DateTime LastOrderDate { get; set; }
        public string PerformanceGrade { get; set; }
        public string RecommendedAction { get; set; }
    }

    public class ExpenseAnalysisModel
    {
        public string ExpenseCategory { get; set; }
        public decimal CurrentPeriodAmount { get; set; }
        public decimal PreviousPeriodAmount { get; set; }
        public decimal VarianceAmount { get; set; }
        public decimal VariancePercentage { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal BudgetVariance { get; set; }
        public decimal BudgetVariancePercentage { get; set; }
        public decimal YearToDateAmount { get; set; }
        public decimal AverageMonthlyAmount { get; set; }
        public string TrendDirection { get; set; }
        public List<MonthlyExpenseBreakdown> MonthlyBreakdown { get; set; }
    }

    public class MonthlyExpenseBreakdown
    {
        public DateTime Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class PurchaseTrendModel
    {
        public DateTime Month { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public int TotalPurchaseOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public int UniqueSuppliers { get; set; }
        public string TopProduct { get; set; }
        public int TopProductQuantity { get; set; }
        public decimal TopProductValue { get; set; }
        public decimal SeasonalityIndex { get; set; }
        public string TrendDirection { get; set; }
    }
}
