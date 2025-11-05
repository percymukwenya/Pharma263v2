using System.Collections.Generic;

namespace Pharma263.MVC.Models
{
    public class DashboardViewModel
    {
        public int Suppliers { get; set; }
        public int Customers { get; set; }
        public int Purchases { get; set; }
        public decimal TotalPurchases { get; set; }
        public int Medicines { get; set; }
        public int Stocks { get; set; }
        public int StockItems { get; set; }
        public int Sales { get; set; }
        public decimal TotalSales { get; set; }        
        public List<LowStock> LowStocks { get; set; }

        // Trend data for percentage changes
        public decimal? SalesPercentageChange { get; set; }
        public decimal? PurchasesPercentageChange { get; set; }
        public decimal? CustomersPercentageChange { get; set; }
        public decimal? SuppliersPercentageChange { get; set; }
        public decimal? StockPercentageChange { get; set; }
        public decimal? MedicinesPercentageChange { get; set; }

        // Trend directions for UI indicators
        public string SalesTrend { get; set; } = "stable";
        public string PurchasesTrend { get; set; } = "stable";
        public string CustomersTrend { get; set; } = "stable";
        public string SuppliersTrend { get; set; } = "stable";
        public string StockTrend { get; set; } = "stable";
        public string MedicinesTrend { get; set; } = "stable";
    }
}
