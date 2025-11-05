using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class DashboardService : IScopedInjectedService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardResponseModel> GetDashboardData(int lowStockAmount)
        {
            return await _dashboardRepository.GetDashboardData(lowStockAmount);
        }

        public async Task<DashboardResponseModel> GetDashboardDataWithTrends(int lowStockAmount, int comparisonDays)
        {
            var currentData = await _dashboardRepository.GetDashboardData(lowStockAmount);
            var previousData = await _dashboardRepository.GetDashboardDataByDateRange(
                DateTime.Now.AddDays(-comparisonDays * 2), 
                DateTime.Now.AddDays(-comparisonDays), 
                lowStockAmount);

            // Calculate trends and percentage changes
            CalculateTrends(currentData.DashboardCounts, previousData?.DashboardCounts);

            return currentData;
        }

        private void CalculateTrends(DashboardCount current, DashboardCount previous)
        {
            if (previous == null) return;

            // Calculate percentage changes and trends
            current.SalesPercentageChange = CalculatePercentageChange(current.TotalSalesAmount, previous.TotalSalesAmount);
            current.SalesTrend = GetTrendDirection(current.SalesPercentageChange);

            current.PurchasesPercentageChange = CalculatePercentageChange(current.TotalPurchaseAmount, previous.TotalPurchaseAmount);
            current.PurchasesTrend = GetTrendDirection(current.PurchasesPercentageChange);

            current.CustomersPercentageChange = CalculatePercentageChange(current.TotalCustomers, previous.TotalCustomers);
            current.CustomersTrend = GetTrendDirection(current.CustomersPercentageChange);

            current.SuppliersPercentageChange = CalculatePercentageChange(current.TotalSuppliers, previous.TotalSuppliers);
            current.SuppliersTrend = GetTrendDirection(current.SuppliersPercentageChange);

            current.StockPercentageChange = CalculatePercentageChange(current.TotalStockQuantity, previous.TotalStockQuantity);
            current.StockTrend = GetTrendDirection(current.StockPercentageChange);

            current.MedicinesPercentageChange = CalculatePercentageChange(current.TotalMedicines, previous.TotalMedicines);
            current.MedicinesTrend = GetTrendDirection(current.MedicinesPercentageChange);
        }

        private decimal? CalculatePercentageChange(decimal current, decimal previous)
        {
            if (previous == 0) return null;
            return Math.Round(((current - previous) / previous) * 100, 1);
        }

        private decimal? CalculatePercentageChange(int current, int previous)
        {
            if (previous == 0) return null;
            return Math.Round(((decimal)(current - previous) / previous) * 100, 1);
        }

        private string GetTrendDirection(decimal? percentageChange)
        {
            if (!percentageChange.HasValue) return "stable";
            
            return percentageChange switch
            {
                > 5 => "up",
                < -5 => "down",
                _ => "stable"
            };
        }
    }
}
