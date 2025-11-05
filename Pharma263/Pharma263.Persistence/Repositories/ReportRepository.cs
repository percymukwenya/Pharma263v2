using Dapper;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private DapperContext _context;

        public ReportRepository(DapperContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
        }

        public async Task<List<AccountsPayableReportModel>> GenerateAccountsPayableReport()
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GenerateAccountsPayableReport]", new
            {

            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<AccountsPayableReportModel>();

            return response.ToList();
        }

        public async Task<List<PurchaseByProductReportModel>> GetPurchaseByProduct(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetPurchaseByProduct]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<PurchaseByProductReportModel>();

            return response.ToList();
        }

        public async Task<PurchaseSummaryModel> GeneratePurchaseSummaryReport(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetPurchaseSummary]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            {
                return resultSet.Read<PurchaseSummaryModel>().FirstOrDefault();
            }
        }

        public async Task<List<SuppliersOwedSummaryModel>> GetSuppliersOwed()
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetSuppliersOwed]", new
            {
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SuppliersOwedSummaryModel>();

            return response.ToList();
        }

        public async Task<List<SupplierPaymentHistoryModel>> GetSupplierPaymentHistory(int supplierId)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetSupplierPaymentHistory]", new
            {
                supplierId
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SupplierPaymentHistoryModel>();

            return response.ToList();
        }

        public async Task<List<PurchaseBySupplierModel>> GetPurchaseBySupplierId(int supplierId)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetPurchaseBySupplier]", new
            {
                supplierId
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<PurchaseBySupplierModel>();

            return response.ToList();
        }


        public async Task<List<AccountsReceivableReportModel>> GenerateAccountsReceivableReport()
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GenerateAccountsReceivableReport]", new
            {

            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<AccountsReceivableReportModel>();

            return response.ToList();
        }

        public async Task<List<SalesByProductReportModel>> GenerateSalesByProductReport(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GenerateSalesByProductReport]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SalesByProductReportModel>();

            return response.ToList();
        }

        public async Task<SalesSummaryModel> GenerateSalesSummaryReport(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GenerateSalesSummaryReport]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            {
                return resultSet.Read<SalesSummaryModel>().FirstOrDefault();
            }
        }

        public async Task<List<CustomersOwingSummaryModel>> GetCustomersOwing()
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetSuppliersOwed]", new
            {
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CustomersOwingSummaryModel>();

            return response.ToList();
        }

        public async Task<List<CustomerPaymentHistoryModel>> GetCustomerPaymentHistory(int customerId)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetCustomerPaymentHistory]", new
            {
                customerId
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CustomerPaymentHistoryModel>();

            return response.ToList();
        }

        public async Task<List<SalesByCustomerModel>> GetSalesByCustomerId(int customerId)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[GetSalesByCustomer]", new
            {
                customerId
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SalesByCustomerModel>();

            return response.ToList();
        }


        public async Task<List<MonthlySalesTrendModel>> GenerateMonthlySalesTrends(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_MonthlySalesTrends]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<MonthlySalesTrendModel>();
            return response.ToList();
        }

        public async Task<List<SalesRepPerformanceModel>> GetSalesRepPerformance(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_SalesRepPerformance]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SalesRepPerformanceModel>();
            return response.ToList();
        }

        public async Task<List<StockSummaryModel>> GenerateStockSummary(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_StockSummary]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<StockSummaryModel>();
            return response.ToList();
        }

        public async Task<List<InventoryAgingModel>> GenerateInventoryAging(DateTime asOfDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_InventoryAging]", new
            {
                asOfDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<InventoryAgingModel>();
            return response.ToList();
        }

        public async Task<List<ExpiryTrackingModel>> GenerateExpiryTracking(int monthsToExpiry)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_ExpiryTracking]", new
            {
                monthsToExpiry
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<ExpiryTrackingModel>();
            return response.ToList();
        }

        public async Task<List<ABCAnalysisModel>> GenerateABCAnalysis(DateTime asOfDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_ABCAnalysis]", new
            {
                asOfDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<ABCAnalysisModel>();
            return response.ToList();
        }

        public async Task<List<CustomerLifetimeValueModel>> GenerateCustomerLifetimeValue(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_CustomerLifetimeValue]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CustomerLifetimeValueModel>();
            return response.ToList();
        }

        public async Task<List<CustomerRetentionModel>> GenerateCustomerRetention(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_CustomerRetention]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CustomerRetentionModel>();
            return response.ToList();
        }

        public async Task<List<CustomerSegmentationModel>> GenerateCustomerSegmentation(DateTime asOfDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_CustomerSegmentation]", new
            {
                asOfDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CustomerSegmentationModel>();
            return response.ToList();
        }

        public async Task<List<ProfitMarginModel>> GenerateProfitMarginAnalysis(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_ProfitMarginAnalysis]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<ProfitMarginModel>();
            return response.ToList();
        }

        public async Task<List<CashFlowAnalysisModel>> GenerateCashFlowAnalysis(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_CashFlowAnalysis]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<CashFlowAnalysisModel>();
            return response.ToList();
        }

        public async Task<List<InventoryTurnoverModel>> GenerateInventoryTurnover(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_StockTurnover]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<InventoryTurnoverModel>();

            return response.ToList();
        }

        public async Task<List<SupplierPerformanceModel>> GenerateSupplierPerformance(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_SupplierPerformance]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<SupplierPerformanceModel>();
            return response.ToList();
        }

        public async Task<List<ExpenseAnalysisModel>> GenerateExpenseAnalysis(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_ExpenseAnalysis]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<ExpenseAnalysisModel>();
            return response.ToList();
        }

        public async Task<List<PurchaseTrendModel>> GeneratePurchaseTrends(DateTime startDate, DateTime endDate)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_PurchaseTrends]", new
            {
                startDate,
                endDate
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);

            var response = await resultSet.ReadAsync<PurchaseTrendModel>();
            return response.ToList();
        }
    }
}
