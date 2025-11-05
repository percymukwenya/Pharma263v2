using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs;
using Pharma263.MVC.Models;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class ReportService : BaseService, IReportService
    {
        private string _pharmaUrl;

        public ReportService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _pharmaUrl = configuration.GetValue<string>("ServiceUrls:BaseAddress");
        }

        public async Task<List<AccountsPayableReportViewModel>> GenerateAccountsPayableReport(string token)
        {
            var response = await this.SendAsync<ApiResponse<List<AccountsPayableReportViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + "/api/Report/GenerateAccountsPayableReport",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<PurchaseByProductReportViewModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<PurchaseByProductReportViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetPurchaseByProduct?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<PurchaseSummaryViewModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<PurchaseSummaryViewModel>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GeneratePurchaseSummaryReport?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SuppliersOwedSummaryViewModel>> GetSuppliersOwed(string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SuppliersOwedSummaryViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + "/api/Report/GetSuppliersOwed",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SupplierPaymentHistoryVM>> GetSupplierPaymentHistory(int supplierId, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SupplierPaymentHistoryVM>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetSupplierPaymentHistory?supplierId={supplierId}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<PurchaseBySupplierViewModel>> GetPurchaseBySupplierId(int supplierId, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<PurchaseBySupplierViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetPurchaseBySupplierId?supplierId={supplierId}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<AccountsReceivableReportViewModel>> GenerateAccountsReceivableReport(string token)
        {
            var response = await this.SendAsync<ApiResponse<List<AccountsReceivableReportViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + "/api/Report/GenerateAccountsReceivableReport",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SalesByProductReportViewModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SalesByProductReportViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetSalesByProduct?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<SalesSummaryViewModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<SalesSummaryViewModel>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GenerateSalesSummaryReport?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<CustomersOwingSummaryViewModel>> GetCustomersOwing(string token)
        {
            var response = await this.SendAsync<ApiResponse<List<CustomersOwingSummaryViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + "/api/Report/GetCustomersOwing",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<CustomerPaymentHistoryVM>> GetCustomerPaymentHistory(int customerId, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<CustomerPaymentHistoryVM>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetCustomerPaymentHistory?customerId={customerId}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SalesByCustomerViewModel>> GetSalesByCustomerId(int customerId, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SalesByCustomerViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetSalesByCustomerId?customerId={customerId}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<MonthlySalesTrendViewModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<MonthlySalesTrendViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/monthly-sales-trends?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SalesRepPerformanceViewModel>> GenerateSalesRepPerformance(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SalesRepPerformanceViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/GetSalesRepPerformance?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<ABCAnalysisViewModel>> GenerateABCAnalysis(DateTime asOfDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<ABCAnalysisViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/abc-analysis?asOfDate={asOfDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<InventoryAgingViewModel>> GenerateInventoryAging(DateTime asOfDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<InventoryAgingViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/inventory-aging?asOfDate={asOfDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<ExpiryTrackingViewModel>> GenerateExpiryTracking(int monthsToExpiry, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<ExpiryTrackingViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/expiry-tracking?monthsToExpiry={monthsToExpiry}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<StockTurnoverViewModel>> GenerateStockTurnover(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<StockTurnoverViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/stock-summary?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<ProfitMarginViewModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<ProfitMarginViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/profit-margin-analysis?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<ExpenseAnalysisViewModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<ExpenseAnalysisViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/expense-analysis?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<CustomerLifetimeValueViewModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<CustomerLifetimeValueViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/customer-lifetime-value?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<CustomerRetentionViewModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<CustomerRetentionViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/customer-retention?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<CustomerSegmentationViewModel>> GenerateCustomerSegmentation(DateTime asOfDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<CustomerSegmentationViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/customer-segmentation?asOfDate={asOfDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<SupplierPerformanceViewModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<SupplierPerformanceViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/supplier-performance?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<PurchaseTrendsViewModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate, string token)
        {
            var response = await this.SendAsync<ApiResponse<List<PurchaseTrendsViewModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/purchase-trends?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}",
                Token = token
            });
            return response.Data;
        }

        public async Task<List<InventoryTurnoverModel>> GenerateStockSummaryReport(DateTime parsedStartDate, DateTime parsedEndDate)
        {
            var response = await this.SendAsync<ApiResponse<List<InventoryTurnoverModel>>>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = _pharmaUrl + $"/api/Report/stock-summary?startDate={parsedStartDate:yyyy-MM-dd}&endDate={parsedEndDate:yyyy-MM-dd}"
            });

            return response.Data;
        }

        public void InvalidateAllReportCaches()
        {
            // For MVC service layer, we don't implement caching - this is handled at the API layer
            // This method exists for interface compliance but is essentially a no-op in this context
        }
    }
}