using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Application.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;
        private readonly IReportAnalyticsService _analyticsService;

        public ReportController(IReportService service, IReportAnalyticsService analyticsService)
        {
            _service = service;
            _analyticsService = analyticsService;
        }

        [HttpGet("GenerateAccountsPayableReport")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<AccountsPayableReportModel>>>> GenerateAccountsPayableReport()
        {
            try
            {
                var response = await _service.GenerateAccountsPayableReport();
                return Ok(ApiResponse<List<AccountsPayableReportModel>>.CreateSuccess(response, "Accounts payable report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AccountsPayableReportModel>>.CreateFailure($"Error generating accounts payable report: {ex.Message}", 500));
            }
        }

        [HttpGet("GenerateAccountsReceivableReport")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<AccountsReceivableReportModel>>>> GenerateAccountsReceivableReport()
        {
            try
            {
                var response = await _service.GenerateAccountsReceivableReport();
                return Ok(ApiResponse<List<AccountsReceivableReportModel>>.CreateSuccess(response, "Accounts receivable report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AccountsReceivableReportModel>>.CreateFailure($"Error generating accounts receivable report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetSalesByProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SalesByProductReportModel>>>> GetSalesByProduct(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<List<SalesByProductReportModel>>(
                    "Invalid date parameters", ModelState));
            }

            if (startDate > endDate)
            {
                return BadRequest(ApiResponse<List<SalesByProductReportModel>>.CreateFailure("Start date cannot be greater than end date", 400));
            }

            if (endDate > DateTime.Now)
            {
                return BadRequest(ApiResponse<List<SalesByProductReportModel>>.CreateFailure("End date cannot be in the future", 400));
            }

            try
            {
                var response = await _service.GenerateSalesByProductReport(startDate, endDate);
                return Ok(ApiResponse<List<SalesByProductReportModel>>.CreateSuccess(response, "Sales by product report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SalesByProductReportModel>>.CreateFailure($"Error generating sales by product report: {ex.Message}", 500));
            }
        }

        [HttpGet("GenerateSalesSummaryReport")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<SalesSummaryModel>>> GenerateSalesSummaryReport(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<SalesSummaryModel>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var response = await _service.GenerateSalesSummaryReport(startDate, endDate);
                return Ok(ApiResponse<SalesSummaryModel>.CreateSuccess(response, "Sales summary report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SalesSummaryModel>.CreateFailure($"Error generating sales summary report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetSalesByCustomerId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SalesByCustomerModel>>>> GetSalesByCustomerId([FromQuery] int customerId)
        {
            try
            {
                var response = await _service.GetSalesByCustomerId(customerId);
                return Ok(ApiResponse<List<SalesByCustomerModel>>.CreateSuccess(response, "Sales by customer report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SalesByCustomerModel>>.CreateFailure($"Error generating sales by customer report: {ex.Message}", 500));
            }
        }

        [HttpGet("monthly-sales-trends")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<MonthlySalesTrendModel>>>> GetMonthlySalesTrends([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GenerateMonthlySalesTrends(startDate, endDate);
                return Ok(ApiResponse<List<MonthlySalesTrendModel>>.CreateSuccess(response, "Monthly sales trends report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<MonthlySalesTrendModel>>.CreateFailure($"Error generating monthly sales trends report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetSalesRepPerformance")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SalesRepPerformanceModel>>>> GetSalesRepPerformance([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GetSalesRepPerformance(startDate, endDate);
                return Ok(ApiResponse<List<SalesRepPerformanceModel>>.CreateSuccess(response, "Sales representative performance report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SalesRepPerformanceModel>>.CreateFailure($"Error generating sales representative performance report: {ex.Message}", 500));
            }
        }

        [HttpGet("GeneratePurchaseSummaryReport")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<PurchaseSummaryModel>>> GeneratePurchaseSummaryReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GeneratePurchaseSummaryReport(startDate, endDate);
                return Ok(ApiResponse<PurchaseSummaryModel>.CreateSuccess(response, "Purchase summary report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PurchaseSummaryModel>.CreateFailure($"Error generating purchase summary report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetPurchaseByProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<PurchaseByProductReportModel>>>> GetPurchaseByProduct([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GetPurchaseByProduct(startDate, endDate);
                return Ok(ApiResponse<List<PurchaseByProductReportModel>>.CreateSuccess(response, "Purchase by product report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<PurchaseByProductReportModel>>.CreateFailure($"Error generating purchase by product report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetPurchaseBySupplierId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<PurchaseBySupplierModel>>>> GetPurchaseBySupplierId([FromQuery] int supplierId)
        {
            try
            {
                var response = await _service.GetPurchaseBySupplierId(supplierId);
                return Ok(ApiResponse<List<PurchaseBySupplierModel>>.CreateSuccess(response, "Purchase by supplier report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<PurchaseBySupplierModel>>.CreateFailure($"Error generating purchase by supplier report: {ex.Message}", 500));
            }
        }

        //[HttpGet("GetSalesRepPerformance")]
        //public async Task<ActionResult<List<SalesRepPerformanceModel>>> GetSalesRepPerformance([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        //{
        //    var response = await _service.GetSalesRepPerformance(startDate, endDate);

        //    return Ok(response);
        //}


        [HttpGet("GetSuppliersOwed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SuppliersOwedSummaryModel>>>> GetSuppliersOwed()
        {
            try
            {
                var response = await _service.GetSuppliersOwed();
                return Ok(ApiResponse<List<SuppliersOwedSummaryModel>>.CreateSuccess(response, "Suppliers owed report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SuppliersOwedSummaryModel>>.CreateFailure($"Error generating suppliers owed report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetSupplierPaymentHistory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SupplierPaymentHistoryModel>>>> GetSupplierPaymentHistory([FromQuery] int supplierId)
        {
            try
            {
                var response = await _service.GetSupplierPaymentHistory(supplierId);
                return Ok(ApiResponse<List<SupplierPaymentHistoryModel>>.CreateSuccess(response, "Supplier payment history report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SupplierPaymentHistoryModel>>.CreateFailure($"Error generating supplier payment history report: {ex.Message}", 500));
            }
        }

        



        [HttpGet("GetCustomersOwing")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<CustomersOwingSummaryModel>>>> GetCustomersOwing()
        {
            try
            {
                var response = await _service.GetCustomersOwing();
                return Ok(ApiResponse<List<CustomersOwingSummaryModel>>.CreateSuccess(response, "Customers owing report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CustomersOwingSummaryModel>>.CreateFailure($"Error generating customers owing report: {ex.Message}", 500));
            }
        }

        [HttpGet("GetCustomerPaymentHistory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<CustomerPaymentHistoryModel>>>> GetCustomerPaymentHistory([FromQuery] int customerId)
        {
            try
            {
                var response = await _service.GetCustomerPaymentHistory(customerId);
                return Ok(ApiResponse<List<CustomerPaymentHistoryModel>>.CreateSuccess(response, "Customer payment history report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CustomerPaymentHistoryModel>>.CreateFailure($"Error generating customer payment history report: {ex.Message}", 500));
            }
        }






        [HttpGet("stock-summary")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<StockSummaryModel>>>> GetStockSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GenerateStockSummary(startDate, endDate);
                return Ok(ApiResponse<List<StockSummaryModel>>.CreateSuccess(response, "Stock summary report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<StockSummaryModel>>.CreateFailure($"Error generating stock summary report: {ex.Message}", 500));
            }
        }

        [HttpGet("inventory-aging")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<InventoryAgingModel>>>> GetInventoryAging([FromQuery] DateTime asOfDate)
        {
            try
            {
                var response = await _service.GenerateInventoryAging(asOfDate);
                return Ok(ApiResponse<List<InventoryAgingModel>>.CreateSuccess(response, "Inventory aging report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<InventoryAgingModel>>.CreateFailure($"Error generating inventory aging report: {ex.Message}", 500));
            }
        }





        [HttpGet("abc-analysis")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<ABCAnalysisModel>>>> GetABCAnalysis([FromQuery] DateTime asOfDate)
        {
            try
            {
                var response = await _service.GenerateABCAnalysis(asOfDate);
                return Ok(ApiResponse<List<ABCAnalysisModel>>.CreateSuccess(response, "ABC analysis report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ABCAnalysisModel>>.CreateFailure($"Error generating ABC analysis report: {ex.Message}", 500));
            }
        }


        [HttpGet("profit-margin-analysis")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<ProfitMarginModel>>>> GetProfitMarginAnalysis([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GenerateProfitMarginAnalysis(startDate, endDate);
                return Ok(ApiResponse<List<ProfitMarginModel>>.CreateSuccess(response, "Profit margin analysis report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ProfitMarginModel>>.CreateFailure($"Error generating profit margin analysis report: {ex.Message}", 500));
            }
        }


        [HttpGet("customer-retention")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<CustomerRetentionModel>>>> GetCustomerRetention([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GenerateCustomerRetention(startDate, endDate);
                return Ok(ApiResponse<List<CustomerRetentionModel>>.CreateSuccess(response, "Customer retention report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CustomerRetentionModel>>.CreateFailure($"Error generating customer retention report: {ex.Message}", 500));
            }
        }

        [HttpGet("customer-segmentation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<CustomerSegmentationModel>>>> GetCustomerSegmentation([FromQuery] DateTime asOfDate)
        {
            try
            {
                var response = await _service.GenerateCustomerSegmentation(asOfDate);
                return Ok(ApiResponse<List<CustomerSegmentationModel>>.CreateSuccess(response, "Customer segmentation report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CustomerSegmentationModel>>.CreateFailure($"Error generating customer segmentation report: {ex.Message}", 500));
            }
        }

        [HttpGet("customer-lifetime-value")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<CustomerLifetimeValueModel>>>> GetCustomerLifetimeValue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var response = await _service.GenerateCustomerLifetimeValue(startDate, endDate);
                return Ok(ApiResponse<List<CustomerLifetimeValueModel>>.CreateSuccess(response, "Customer lifetime value report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CustomerLifetimeValueModel>>.CreateFailure($"Error generating customer lifetime value report: {ex.Message}", 500));
            }
        }

        #region Advanced Analytics Endpoints

        [HttpGet("dashboard-summary")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<DashboardSummaryModel>>> GetDashboardSummary()
        {
            try
            {
                var summary = await _analyticsService.GenerateDashboardSummary();
                return Ok(ApiResponse<DashboardSummaryModel>.CreateSuccess(summary, "Dashboard summary generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DashboardSummaryModel>.CreateFailure($"Error generating dashboard summary: {ex.Message}", 500));
            }
        }

        [HttpGet("system-analytics")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<SystemAnalyticsModel>>> GetSystemAnalytics(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<SystemAnalyticsModel>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var analytics = await _analyticsService.GenerateSystemAnalytics(startDate, endDate);
                return Ok(ApiResponse<SystemAnalyticsModel>.CreateSuccess(analytics, "System analytics generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SystemAnalyticsModel>.CreateFailure($"Error generating system analytics: {ex.Message}", 500));
            }
        }

        [HttpGet("business-insights")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<BusinessInsightsModel>>> GetBusinessInsights(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<BusinessInsightsModel>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var insights = await _analyticsService.GenerateBusinessInsights(startDate, endDate);
                return Ok(ApiResponse<BusinessInsightsModel>.CreateSuccess(insights, "Business insights generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BusinessInsightsModel>.CreateFailure($"Error generating business insights: {ex.Message}", 500));
            }
        }

        [HttpPost("invalidate-cache")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        public ActionResult<ApiResponse<bool>> InvalidateReportCache()
        {
            try
            {
                _service.InvalidateAllReportCaches();
                _analyticsService.InvalidateAnalyticsCache();
                return Ok(ApiResponse<bool>.CreateSuccess(true, "All report caches invalidated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.CreateFailure($"Error invalidating caches: {ex.Message}", 500));
            }
        }

        [HttpGet("supplier-performance")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<SupplierPerformanceModel>>>> GetSupplierPerformance(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<List<SupplierPerformanceModel>>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var response = await _service.GenerateSupplierPerformance(startDate, endDate);
                return Ok(ApiResponse<List<SupplierPerformanceModel>>.CreateSuccess(response, "Supplier performance report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SupplierPerformanceModel>>.CreateFailure($"Error generating supplier performance report: {ex.Message}", 500));
            }
        }

        [HttpGet("purchase-trends")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<PurchaseTrendModel>>>> GetPurchaseTrends(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<List<PurchaseTrendModel>>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var response = await _service.GeneratePurchaseTrends(startDate, endDate);
                return Ok(ApiResponse<List<PurchaseTrendModel>>.CreateSuccess(response, "Purchase trends report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<PurchaseTrendModel>>.CreateFailure($"Error generating purchase trends report: {ex.Message}", 500));
            }
        }

        [HttpGet("expense-analysis")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<ExpenseAnalysisModel>>>> GetExpenseAnalysis(
            [FromQuery][Required] DateTime startDate, 
            [FromQuery][Required] DateTime endDate)
        {
            var dateValidationResult = ValidateDateRange<List<ExpenseAnalysisModel>>(startDate, endDate);
            if (dateValidationResult != null)
                return dateValidationResult;

            try
            {
                var response = await _service.GenerateExpenseAnalysis(startDate, endDate);
                return Ok(ApiResponse<List<ExpenseAnalysisModel>>.CreateSuccess(response, "Expense analysis report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ExpenseAnalysisModel>>.CreateFailure($"Error generating expense analysis report: {ex.Message}", 500));
            }
        }

        [HttpGet("expiry-tracking")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse<List<ExpiryTrackingModel>>>> GetExpiryTracking(
            [FromQuery][Required] int monthsToExpiry)
        {
            try
            {
                var response = await _service.GenerateExpiryTracking(monthsToExpiry);
                return Ok(ApiResponse<List<ExpiryTrackingModel>>.CreateSuccess(response, "Expiry tracking report generated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ExpiryTrackingModel>>.CreateFailure($"Error generating expiry tracking report: {ex.Message}", 500));
            }
        }

        #endregion

        private BadRequestObjectResult ValidateDateRange<T>(DateTime startDate, DateTime endDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<T>("Invalid date parameters", ModelState));
            }

            if (startDate > endDate)
            {
                return BadRequest(ApiResponse<T>.CreateFailure("Start date cannot be greater than end date", 400));
            }

            if (endDate > DateTime.Now)
            {
                return BadRequest(ApiResponse<T>.CreateFailure("End date cannot be in the future", 400));
            }

            if (startDate < DateTime.Now.AddYears(-5))
            {
                return BadRequest(ApiResponse<T>.CreateFailure("Start date cannot be more than 5 years ago", 400));
            }

            return null;
        }

        private BadRequestObjectResult ValidateId(int id, string idName)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponse<object>.CreateFailure($"{idName} must be greater than 0", 400));
            }
            return null;
        }
    }
}
