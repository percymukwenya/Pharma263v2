namespace Pharma263.Domain.Models
{
    public class DashboardCount
    {
        public int TotalCustomers { get; set; }
        public int TotalMedicines { get; set; }
        public int TotalStockItems { get; set; }
        public int TotalStockQuantity { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalPurchases { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalSalesAmount { get; set; }

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
