using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models
{
    /// <summary>
    /// Comprehensive dashboard summary with key business metrics
    /// </summary>
    public class DashboardSummaryModel
    {
        public SalesMetrics Sales { get; set; }
        public InventoryMetrics Inventory { get; set; }
        public FinancialMetrics Financial { get; set; }
        public UserActivityMetrics UserActivity { get; set; }
        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class SalesMetrics
    {
        public decimal TodayRevenue { get; set; }
        public decimal WeekRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public int TodayTransactions { get; set; }
        public int WeekTransactions { get; set; }
        public int MonthTransactions { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal RevenueGrowthPercent { get; set; }
    }

    public class InventoryMetrics
    {
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public int ExpiringThisMonthCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public decimal InventoryTurnoverRatio { get; set; }
    }

    public class FinancialMetrics
    {
        public decimal TotalAccountsReceivable { get; set; }
        public decimal TotalAccountsPayable { get; set; }
        public decimal OverdueReceivables { get; set; }
        public decimal OverduePayables { get; set; }
        public decimal CashFlow { get; set; }
        public decimal ProfitMargin { get; set; }
    }

    public class UserActivityMetrics
    {
        public int ActiveUsersToday { get; set; }
        public int ActiveUsersThisWeek { get; set; }
        public int TotalUsers { get; set; }
        public DateTime LastSystemBackup { get; set; }
        public int RecentErrorsCount { get; set; }
    }

    public class AlertModel
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; } // Low, Medium, High, Critical
        public DateTime CreatedAt { get; set; }
        public string ActionRequired { get; set; }
    }
}