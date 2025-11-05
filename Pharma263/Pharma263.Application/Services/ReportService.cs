using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ReportService> _logger;
        
        // Cache keys
        private const string AccountsPayableCacheKey = "accounts_payable_report";
        private const string AccountsReceivableCacheKey = "accounts_receivable_report";
        private const string SalesByProductCacheKeyPrefix = "sales_by_product_";
        private const string SalesSummaryCacheKeyPrefix = "sales_summary_";
        private const string StockSummaryCacheKeyPrefix = "stock_summary_";
        private const string InventoryAgingCacheKeyPrefix = "inventory_aging_";
        
        // Cache expiry times - longer for less frequently changing data
        private readonly TimeSpan ShortCacheExpiry = TimeSpan.FromMinutes(15); // For transactional reports
        private readonly TimeSpan MediumCacheExpiry = TimeSpan.FromMinutes(30); // For summary reports  
        private readonly TimeSpan LongCacheExpiry = TimeSpan.FromHours(2); // For analytical reports

        public ReportService(IReportRepository reportRepository, IMemoryCache memoryCache, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<List<ABCAnalysisModel>> GenerateABCAnalysis(DateTime asOfDate)
        {
            return await _reportRepository.GenerateABCAnalysis(asOfDate);
        }

        public async Task<List<AccountsPayableReportModel>> GenerateAccountsPayableReport()
        {
            try
            {
                if (_memoryCache.TryGetValue(AccountsPayableCacheKey, out List<AccountsPayableReportModel> cached))
                {
                    _logger.LogInformation("Accounts payable report retrieved from cache");
                    return cached;
                }

                var result = await _reportRepository.GenerateAccountsPayableReport();
                _memoryCache.Set(AccountsPayableCacheKey, result, ShortCacheExpiry);
                
                _logger.LogInformation("Accounts payable report generated and cached for {Minutes} minutes", ShortCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating accounts payable report");
                throw;
            }
        }

        public async Task<List<AccountsReceivableReportModel>> GenerateAccountsReceivableReport()
        {
            try
            {
                if (_memoryCache.TryGetValue(AccountsReceivableCacheKey, out List<AccountsReceivableReportModel> cached))
                {
                    _logger.LogInformation("Accounts receivable report retrieved from cache");
                    return cached;
                }

                var result = await _reportRepository.GenerateAccountsReceivableReport();
                _memoryCache.Set(AccountsReceivableCacheKey, result, ShortCacheExpiry);
                
                _logger.LogInformation("Accounts receivable report generated and cached for {Minutes} minutes", ShortCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating accounts receivable report");
                throw;
            }
        }

        public async Task<List<CashFlowAnalysisModel>> GenerateCashFlowAnalysis(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateCashFlowAnalysis(startDate, endDate);
        }

        public async Task<List<CustomerLifetimeValueModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateCustomerLifetimeValue(startDate, endDate);
        }

        public async Task<List<CustomerRetentionModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateCustomerRetention(startDate, endDate);
        }

        public async Task<List<CustomerSegmentationModel>> GenerateCustomerSegmentation(DateTime asOfDate)
        {
            return await _reportRepository.GenerateCustomerSegmentation(asOfDate);
        }

        public async Task<List<ExpiryTrackingModel>> GenerateExpiryTracking(int monthsToExpiry)
        {
            return await _reportRepository.GenerateExpiryTracking(monthsToExpiry);
        }

        public async Task<List<InventoryAgingModel>> GenerateInventoryAging(DateTime asOfDate)
        {
            return await _reportRepository.GenerateInventoryAging(asOfDate);
        }

        public async Task<List<InventoryTurnoverModel>> GenerateInventoryTurnover(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateInventoryTurnover(startDate, endDate);
        }

        public async Task<List<MonthlySalesTrendModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateMonthlySalesTrends(startDate, endDate);
        }

        public async Task<List<ProfitMarginModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GenerateProfitMarginAnalysis(startDate, endDate);
        }

        public async Task<PurchaseSummaryModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GeneratePurchaseSummaryReport(startDate, endDate);
        }

        public async Task<List<SalesByProductReportModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"{SalesByProductCacheKeyPrefix}{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out List<SalesByProductReportModel> cached))
                {
                    _logger.LogInformation("Sales by product report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GenerateSalesByProductReport(startDate, endDate);
                _memoryCache.Set(cacheKey, result, MediumCacheExpiry);
                
                _logger.LogInformation("Sales by product report generated and cached for {Minutes} minutes", MediumCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales by product report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<SalesSummaryModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"{SalesSummaryCacheKeyPrefix}{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out SalesSummaryModel cached))
                {
                    _logger.LogInformation("Sales summary report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GenerateSalesSummaryReport(startDate, endDate);
                _memoryCache.Set(cacheKey, result, MediumCacheExpiry);
                
                _logger.LogInformation("Sales summary report generated and cached for {Minutes} minutes", MediumCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating sales summary report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<List<StockSummaryModel>> GenerateStockSummary(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"{StockSummaryCacheKeyPrefix}{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out List<StockSummaryModel> cached))
                {
                    _logger.LogInformation("Stock summary report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GenerateStockSummary(startDate, endDate);
                _memoryCache.Set(cacheKey, result, LongCacheExpiry);
                
                _logger.LogInformation("Stock summary report generated and cached for {Hours} hours", LongCacheExpiry.TotalHours);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating stock summary report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<List<CustomerPaymentHistoryModel>> GetCustomerPaymentHistory(int customerId)
        {
            return await _reportRepository.GetCustomerPaymentHistory(customerId);
        }

        public async Task<List<CustomersOwingSummaryModel>> GetCustomersOwing()
        {
            return await _reportRepository.GetCustomersOwing();
        }

        public async Task<List<PurchaseByProductReportModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GetPurchaseByProduct(startDate, endDate);
        }

        public async Task<List<PurchaseBySupplierModel>> GetPurchaseBySupplierId(int supplierId)
        {
            return await _reportRepository.GetPurchaseBySupplierId(supplierId);
        }

        public async Task<List<SalesByCustomerModel>> GetSalesByCustomerId(int customerId)
        {
            return await _reportRepository.GetSalesByCustomerId(customerId);
        }

        public async Task<List<SalesRepPerformanceModel>> GetSalesRepPerformance(DateTime startDate, DateTime endDate)
        {
            return await _reportRepository.GetSalesRepPerformance(startDate, endDate);
        }

        public async Task<List<SupplierPaymentHistoryModel>> GetSupplierPaymentHistory(int supplierId)
        {
            return await _reportRepository.GetSupplierPaymentHistory(supplierId);
        }

        public Task<List<SuppliersOwedSummaryModel>> GetSuppliersOwed()
        {
            return _reportRepository.GetSuppliersOwed();
        }

        public async Task<List<SupplierPerformanceModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"supplier_performance_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out List<SupplierPerformanceModel> cached))
                {
                    _logger.LogInformation("Supplier performance report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GenerateSupplierPerformance(startDate, endDate);
                _memoryCache.Set(cacheKey, result, MediumCacheExpiry);
                
                _logger.LogInformation("Supplier performance report generated and cached for {Minutes} minutes", MediumCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating supplier performance report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<List<ExpenseAnalysisModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"expense_analysis_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out List<ExpenseAnalysisModel> cached))
                {
                    _logger.LogInformation("Expense analysis report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GenerateExpenseAnalysis(startDate, endDate);
                _memoryCache.Set(cacheKey, result, MediumCacheExpiry);
                
                _logger.LogInformation("Expense analysis report generated and cached for {Minutes} minutes", MediumCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating expense analysis report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<List<PurchaseTrendModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"purchase_trends_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                
                if (_memoryCache.TryGetValue(cacheKey, out List<PurchaseTrendModel> cached))
                {
                    _logger.LogInformation("Purchase trends report retrieved from cache for period {StartDate} to {EndDate}", startDate, endDate);
                    return cached;
                }

                var result = await _reportRepository.GeneratePurchaseTrends(startDate, endDate);
                _memoryCache.Set(cacheKey, result, MediumCacheExpiry);
                
                _logger.LogInformation("Purchase trends report generated and cached for {Minutes} minutes", MediumCacheExpiry.TotalMinutes);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating purchase trends report for period {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }

        /// <summary>
        /// Invalidates all cached reports - useful when data changes significantly
        /// </summary>
        public void InvalidateAllReportCaches()
        {
            try
            {
                var keys = new List<object>();
                
                // Get all cache keys that start with report prefixes
                if (_memoryCache is MemoryCache memCache)
                {
                    var field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var coherentState = field?.GetValue(memCache);
                    var entriesCollection = coherentState?.GetType().GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var entries = (IDictionary)entriesCollection?.GetValue(coherentState);

                    if (entries != null)
                    {
                        foreach (DictionaryEntry entry in entries)
                        {
                            var key = entry.Key.ToString();
                            if (key.StartsWith("accounts_") || 
                                key.StartsWith("sales_") || 
                                key.StartsWith("stock_") || 
                                key.StartsWith("inventory_"))
                            {
                                keys.Add(entry.Key);
                            }
                        }
                    }
                }

                foreach (var key in keys)
                {
                    _memoryCache.Remove(key);
                }

                _logger.LogInformation("Invalidated {Count} cached reports", keys.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating report caches");
            }
        }

        /// <summary>
        /// Invalidates specific report type caches
        /// </summary>
        public void InvalidateReportCache(string cacheKeyPrefix)
        {
            try
            {
                _memoryCache.Remove(cacheKeyPrefix);
                _logger.LogInformation("Invalidated cache with key: {CacheKey}", cacheKeyPrefix);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating cache with key: {CacheKey}", cacheKeyPrefix);
            }
        }
    }
}
