using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs;
using Pharma263.MVC.Models;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Service for generating various business reports (sales, purchases, inventory, financial analytics).
    /// Migrated from BaseService to IApiService for better performance and consistency.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IApiService _apiService;

        public ReportService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<AccountsPayableReportViewModel>> GenerateAccountsPayableReport(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            // IApiService automatically injects token via ITokenService
            var response = await _apiService.GetApiResponseAsync<List<AccountsPayableReportViewModel>>("/api/Report/GenerateAccountsPayableReport");
            return response.Data;
        }

        public async Task<List<PurchaseByProductReportViewModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<PurchaseByProductReportViewModel>>(
                $"/api/Report/GetPurchaseByProduct?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<PurchaseSummaryViewModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<PurchaseSummaryViewModel>(
                $"/api/Report/GeneratePurchaseSummaryReport?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<SuppliersOwedSummaryViewModel>> GetSuppliersOwed(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SuppliersOwedSummaryViewModel>>("/api/Report/GetSuppliersOwed");
            return response.Data;
        }

        public async Task<List<SupplierPaymentHistoryVM>> GetSupplierPaymentHistory(int supplierId, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SupplierPaymentHistoryVM>>(
                $"/api/Report/GetSupplierPaymentHistory?supplierId={supplierId}");
            return response.Data;
        }

        public async Task<List<PurchaseBySupplierViewModel>> GetPurchaseBySupplierId(int supplierId, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<PurchaseBySupplierViewModel>>(
                $"/api/Report/GetPurchaseBySupplierId?supplierId={supplierId}");
            return response.Data;
        }

        public async Task<List<AccountsReceivableReportViewModel>> GenerateAccountsReceivableReport(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<AccountsReceivableReportViewModel>>("/api/Report/GenerateAccountsReceivableReport");
            return response.Data;
        }

        public async Task<List<SalesByProductReportViewModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SalesByProductReportViewModel>>(
                $"/api/Report/GetSalesByProduct?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<SalesSummaryViewModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<SalesSummaryViewModel>(
                $"/api/Report/GenerateSalesSummaryReport?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<CustomersOwingSummaryViewModel>> GetCustomersOwing(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<CustomersOwingSummaryViewModel>>("/api/Report/GetCustomersOwing");
            return response.Data;
        }

        public async Task<List<CustomerPaymentHistoryVM>> GetCustomerPaymentHistory(int customerId, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<CustomerPaymentHistoryVM>>(
                $"/api/Report/GetCustomerPaymentHistory?customerId={customerId}");
            return response.Data;
        }

        public async Task<List<SalesByCustomerViewModel>> GetSalesByCustomerId(int customerId, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SalesByCustomerViewModel>>(
                $"/api/Report/GetSalesByCustomerId?customerId={customerId}");
            return response.Data;
        }

        public async Task<List<MonthlySalesTrendViewModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<MonthlySalesTrendViewModel>>(
                $"/api/Report/monthly-sales-trends?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<SalesRepPerformanceViewModel>> GenerateSalesRepPerformance(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SalesRepPerformanceViewModel>>(
                $"/api/Report/GetSalesRepPerformance?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<ABCAnalysisViewModel>> GenerateABCAnalysis(DateTime asOfDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<ABCAnalysisViewModel>>(
                $"/api/Report/abc-analysis?asOfDate={asOfDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<InventoryAgingViewModel>> GenerateInventoryAging(DateTime asOfDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<InventoryAgingViewModel>>(
                $"/api/Report/inventory-aging?asOfDate={asOfDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<ExpiryTrackingViewModel>> GenerateExpiryTracking(int monthsToExpiry, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<ExpiryTrackingViewModel>>(
                $"/api/Report/expiry-tracking?monthsToExpiry={monthsToExpiry}");
            return response.Data;
        }

        public async Task<List<StockTurnoverViewModel>> GenerateStockTurnover(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<StockTurnoverViewModel>>(
                $"/api/Report/stock-summary?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<ProfitMarginViewModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<ProfitMarginViewModel>>(
                $"/api/Report/profit-margin-analysis?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<ExpenseAnalysisViewModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<ExpenseAnalysisViewModel>>(
                $"/api/Report/expense-analysis?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<CustomerLifetimeValueViewModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<CustomerLifetimeValueViewModel>>(
                $"/api/Report/customer-lifetime-value?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<CustomerRetentionViewModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<CustomerRetentionViewModel>>(
                $"/api/Report/customer-retention?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<CustomerSegmentationViewModel>> GenerateCustomerSegmentation(DateTime asOfDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<CustomerSegmentationViewModel>>(
                $"/api/Report/customer-segmentation?asOfDate={asOfDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<SupplierPerformanceViewModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<SupplierPerformanceViewModel>>(
                $"/api/Report/supplier-performance?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<PurchaseTrendsViewModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate, string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            var response = await _apiService.GetApiResponseAsync<List<PurchaseTrendsViewModel>>(
                $"/api/Report/purchase-trends?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response.Data;
        }

        public async Task<List<InventoryTurnoverModel>> GenerateStockSummaryReport(DateTime parsedStartDate, DateTime parsedEndDate)
        {
            var response = await _apiService.GetApiResponseAsync<List<InventoryTurnoverModel>>(
                $"/api/Report/stock-summary?startDate={parsedStartDate:yyyy-MM-dd}&endDate={parsedEndDate:yyyy-MM-dd}");
            return response.Data;
        }

        public void InvalidateAllReportCaches()
        {
            // For MVC service layer, we don't implement caching - this is handled at the API layer
            // This method exists for interface compliance but is essentially a no-op in this context
        }
    }
}