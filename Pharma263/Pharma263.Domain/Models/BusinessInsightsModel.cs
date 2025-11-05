using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models
{
    /// <summary>
    /// Advanced business insights and analytics
    /// </summary>
    public class BusinessInsightsModel
    {
        public DateTime ReportPeriodStart { get; set; }
        public DateTime ReportPeriodEnd { get; set; }
        public SalesTrendAnalysis SalesTrends { get; set; }
        public CustomerInsights CustomerAnalytics { get; set; }
        public ProductPerformanceAnalysis ProductAnalytics { get; set; }
        public SeasonalityAnalysis SeasonalPatterns { get; set; }
        public List<BusinessRecommendationModel> Recommendations { get; set; } = new List<BusinessRecommendationModel>();
    }

    public class SalesTrendAnalysis
    {
        public decimal GrowthRate { get; set; }
        public List<TrendDataPoint> DailyTrends { get; set; } = new List<TrendDataPoint>();
        public List<TrendDataPoint> WeeklyTrends { get; set; } = new List<TrendDataPoint>();
        public List<TrendDataPoint> MonthlyTrends { get; set; } = new List<TrendDataPoint>();
        public string TrendDirection { get; set; } // Up, Down, Stable
        public decimal ProjectedNextMonthRevenue { get; set; }
        public string Confidence { get; set; } // High, Medium, Low
    }

    public class CustomerInsights
    {
        public int NewCustomersCount { get; set; }
        public int ReturnedCustomersCount { get; set; }
        public decimal CustomerRetentionRate { get; set; }
        public decimal AverageCustomerLifetimeValue { get; set; }
        public List<CustomerSegmentModel> CustomerSegments { get; set; } = new List<CustomerSegmentModel>();
        public List<CustomerBehaviorModel> TopCustomers { get; set; } = new List<CustomerBehaviorModel>();
    }

    public class ProductPerformanceAnalysis
    {
        public List<ProductPerformanceModel> TopSellingProducts { get; set; } = new List<ProductPerformanceModel>();
        public List<ProductPerformanceModel> SlowMovingProducts { get; set; } = new List<ProductPerformanceModel>();
        public List<ProductPerformanceModel> MostProfitableProducts { get; set; } = new List<ProductPerformanceModel>();
        public List<string> ProductsNearExpiry { get; set; } = new List<string>();
        public decimal OverallInventoryTurnover { get; set; }
    }

    public class SeasonalityAnalysis
    {
        public List<SeasonalTrendModel> MonthlyPatterns { get; set; } = new List<SeasonalTrendModel>();
        public List<SeasonalTrendModel> WeeklyPatterns { get; set; } = new List<SeasonalTrendModel>();
        public string PeakSeasonMonth { get; set; }
        public string SlowSeasonMonth { get; set; }
        public decimal SeasonalityStrength { get; set; }
    }

    public class TrendDataPoint
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; }
    }

    public class CustomerSegmentModel
    {
        public string SegmentName { get; set; }
        public int CustomerCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal TotalRevenue { get; set; }
        public string Characteristics { get; set; }
    }

    public class CustomerBehaviorModel
    {
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime LastPurchase { get; set; }
        public string PreferredProducts { get; set; }
        public string RiskLevel { get; set; } // Low, Medium, High (churn risk)
    }

    public class ProductPerformanceModel
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal ProfitMargin { get; set; }
        public int InventoryRemaining { get; set; }
        public decimal TurnoverRate { get; set; }
        public DateTime? LastSaleDate { get; set; }
    }

    public class SeasonalTrendModel
    {
        public string Period { get; set; }
        public decimal AverageValue { get; set; }
        public decimal GrowthRate { get; set; }
        public int TransactionCount { get; set; }
    }

    public class BusinessRecommendationModel
    {
        public string Category { get; set; } // Sales, Inventory, Finance, Marketing
        public string Title { get; set; }
        public string Description { get; set; }
        public string Impact { get; set; } // High, Medium, Low
        public string Effort { get; set; } // High, Medium, Low
        public decimal EstimatedROI { get; set; }
        public string ActionRequired { get; set; }
        public DateTime RecommendedBy { get; set; }
    }
}