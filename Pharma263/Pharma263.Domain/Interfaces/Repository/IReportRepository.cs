using Pharma263.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Domain.Interfaces.Repository
{
    public interface IReportRepository
    {
        Task<List<AccountsPayableReportModel>> GenerateAccountsPayableReport();
        Task<List<PurchaseByProductReportModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate);
        Task<PurchaseSummaryModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate);
        Task<List<SuppliersOwedSummaryModel>> GetSuppliersOwed();
        Task<List<SupplierPaymentHistoryModel>> GetSupplierPaymentHistory(int supplierId);
        Task<List<PurchaseBySupplierModel>> GetPurchaseBySupplierId(int supplierId);


        Task<List<AccountsReceivableReportModel>> GenerateAccountsReceivableReport();
        Task<List<SalesByProductReportModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate);
        Task<SalesSummaryModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate);
        Task<List<CustomersOwingSummaryModel>> GetCustomersOwing();
        Task<List<CustomerPaymentHistoryModel>> GetCustomerPaymentHistory(int customerId);
        Task<List<SalesByCustomerModel>> GetSalesByCustomerId(int customerId);

        Task<List<MonthlySalesTrendModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate);
        Task<List<SalesRepPerformanceModel>> GetSalesRepPerformance(DateTime startDate, DateTime endDate);

        // New methods for Inventory
        Task<List<StockSummaryModel>> GenerateStockSummary(DateTime startDate, DateTime endDate);
        Task<List<InventoryAgingModel>> GenerateInventoryAging(DateTime asOfDate);
        Task<List<ExpiryTrackingModel>> GenerateExpiryTracking(int monthsToExpiry);
        Task<List<ABCAnalysisModel>> GenerateABCAnalysis(DateTime asOfDate);
        Task<List<InventoryTurnoverModel>> GenerateInventoryTurnover(DateTime startDate, DateTime endDate);

        // New methods for Customer Analysis
        Task<List<CustomerLifetimeValueModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate);
        Task<List<CustomerRetentionModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate);
        Task<List<CustomerSegmentationModel>> GenerateCustomerSegmentation(DateTime asOfDate);

        // New methods for Financial Analysis
        Task<List<ProfitMarginModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate);
        Task<List<CashFlowAnalysisModel>> GenerateCashFlowAnalysis(DateTime startDate, DateTime endDate);
        Task<List<ExpenseAnalysisModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate);

        // Purchase Analysis
        Task<List<SupplierPerformanceModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate);
        Task<List<PurchaseTrendModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate);
    }
}
