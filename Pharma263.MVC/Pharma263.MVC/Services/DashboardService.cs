using Pharma263.Integration.Api;
using Pharma263.MVC.Models;
using Pharma263.MVC.Services.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPharmaApiService _pharmaApiService;

        public DashboardService(IPharmaApiService pharmaApiService)
        {
            _pharmaApiService = pharmaApiService;
        }

        public async Task<DashboardViewModel> GetDashboardAsync(string token)
        {
            var response = await _pharmaApiService.GetDashboard(token);

            // Manual mapping - simpler than AutoMapper overhead
            var lowStocks = response.LowStocks?.Select(ls => new LowStock
            {
                MedicineName = ls.Name,
                BatchNo = ls.BatchNo,
                TotalQuantity = ls.TotalQuantity,
                BuyingPrice = (double)ls.BuyingPrice,
                SellingPrice = (double)ls.SellingPrice
            }).ToList() ?? new List<LowStock>();

            return new DashboardViewModel
            {
                Customers = response.DashboardCounts.TotalCustomers,
                Medicines = response.DashboardCounts.TotalMedicines,
                Stocks = response.DashboardCounts.TotalStockQuantity,
                StockItems = response.DashboardCounts.TotalStockItems,
                Suppliers = response.DashboardCounts.TotalSuppliers,
                Purchases = response.DashboardCounts.TotalPurchases,
                TotalPurchases = response.DashboardCounts.TotalPurchaseAmount,
                Sales = response.DashboardCounts.TotalSales,
                TotalSales = response.DashboardCounts.TotalSalesAmount,
                LowStocks = lowStocks
            };
        }

        public async Task<DashboardViewModel> GetDashboardWithTrendsAsync(string token, int days = 30)
        {
            var response = await _pharmaApiService.GetDashboardWithTrends(token, days);

            // Manual mapping - simpler than AutoMapper overhead
            var lowStocks = response.Data.LowStocks?.Select(ls => new LowStock
            {
                MedicineName = ls.Name,
                BatchNo = ls.BatchNo,
                TotalQuantity = ls.TotalQuantity,
                BuyingPrice = (double)ls.BuyingPrice,
                SellingPrice = (double)ls.SellingPrice
            }).ToList() ?? new List<LowStock>();

            return new DashboardViewModel
            {
                Customers = response.Data.DashboardCounts.TotalCustomers,
                Medicines = response.Data.DashboardCounts.TotalMedicines,
                Stocks = response.Data.DashboardCounts.TotalStockQuantity,
                StockItems = response.Data.DashboardCounts.TotalStockItems,
                Suppliers = response.Data.DashboardCounts.TotalSuppliers,
                Purchases = response.Data.DashboardCounts.TotalPurchases,
                TotalPurchases = response.Data.DashboardCounts.TotalPurchaseAmount,
                Sales = response.Data.DashboardCounts.TotalSales,
                TotalSales = response.Data.DashboardCounts.TotalSalesAmount,
                LowStocks = lowStocks,

                // Add trend data
                SalesPercentageChange = response.Data.DashboardCounts.SalesPercentageChange,
                PurchasesPercentageChange = response.Data.DashboardCounts.PurchasesPercentageChange,
                CustomersPercentageChange = response.Data.DashboardCounts.CustomersPercentageChange,
                SuppliersPercentageChange = response.Data.DashboardCounts.SuppliersPercentageChange,
                StockPercentageChange = response.Data.DashboardCounts.StockPercentageChange,
                MedicinesPercentageChange = response.Data.DashboardCounts.MedicinesPercentageChange,

                SalesTrend = response.Data.DashboardCounts.SalesTrend,
                PurchasesTrend = response.Data.DashboardCounts.PurchasesTrend,
                CustomersTrend = response.Data.DashboardCounts.CustomersTrend,
                SuppliersTrend = response.Data.DashboardCounts.SuppliersTrend,
                StockTrend = response.Data.DashboardCounts.StockTrend,
                MedicinesTrend = response.Data.DashboardCounts.MedicinesTrend
            };
        }
    }
}
