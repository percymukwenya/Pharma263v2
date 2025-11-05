using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Pharma263.MVC.Models;

namespace Pharma263.MVC.Services.IService
{
    public interface IReportService
    {
        Task<List<AccountsPayableReportViewModel>> GenerateAccountsPayableReport(string token);
        Task<List<PurchaseByProductReportViewModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate, string token);
        Task<PurchaseSummaryViewModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate, string token);
        Task<List<SuppliersOwedSummaryViewModel>> GetSuppliersOwed(string token);
        Task<List<SupplierPaymentHistoryVM>> GetSupplierPaymentHistory(int supplierId, string token);
        Task<List<PurchaseBySupplierViewModel>> GetPurchaseBySupplierId(int supplierId, string token);


        Task<List<AccountsReceivableReportViewModel>> GenerateAccountsReceivableReport(string token);
        Task<List<SalesByProductReportViewModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate, string token);
        Task<SalesSummaryViewModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate, string token);
        Task<List<CustomersOwingSummaryViewModel>> GetCustomersOwing(string token);
        Task<List<CustomerPaymentHistoryVM>> GetCustomerPaymentHistory(int customerId, string token);
        Task<List<SalesByCustomerViewModel>> GetSalesByCustomerId(int customerId, string token);


        // New Sales Reports
        Task<List<MonthlySalesTrendViewModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate, string token);
        Task<List<SalesRepPerformanceViewModel>> GenerateSalesRepPerformance(DateTime startDate, DateTime endDate, string token);

        // New Inventory Reports
        Task<List<ABCAnalysisViewModel>> GenerateABCAnalysis(DateTime asOfDate, string token);
        Task<List<InventoryAgingViewModel>> GenerateInventoryAging(DateTime asOfDate, string token);
        Task<List<ExpiryTrackingViewModel>> GenerateExpiryTracking(int monthsToExpiry, string token);
        Task<List<StockTurnoverViewModel>> GenerateStockTurnover(DateTime startDate, DateTime endDate, string token);

        // New Financial Reports
        Task<List<ProfitMarginViewModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate, string token);
        Task<List<ExpenseAnalysisViewModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate, string token);

        // New Customer Analysis Reports
        Task<List<CustomerLifetimeValueViewModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate, string token);
        Task<List<CustomerRetentionViewModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate, string token);
        Task<List<CustomerSegmentationViewModel>> GenerateCustomerSegmentation(DateTime asOfDate, string token);

        // New Supplier Analysis Reports
        Task<List<SupplierPerformanceViewModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate, string token);
        Task<List<PurchaseTrendsViewModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate, string token);
        Task<List<InventoryTurnoverModel>> GenerateStockSummaryReport(DateTime parsedStartDate, DateTime parsedEndDate);
        
        // Cache management
        void InvalidateAllReportCaches();
    }
}
