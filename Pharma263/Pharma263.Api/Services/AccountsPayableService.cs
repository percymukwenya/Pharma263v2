using Dapper;
using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models.AccountsPayable.Request;
using Pharma263.Api.Models.AccountsPayable.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Models;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Models.Dtos;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class AccountsPayableService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DapperContext _context;
        private readonly IAppLogger<AccountsPayableService> _logger;

        public AccountsPayableService(IUnitOfWork unitOfWork,
            DapperContext context, IAppLogger<AccountsPayableService> logger)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<AccountsPayableListResponse>>> GetAccountsPayable()
        {
            try
            {
                var accounts = await _unitOfWork.Repository<AccountsPayable>().GetAllAsync(query =>
                query.Include(s => s.Supplier)
                     .Include(a => a.AccountsPayableStatus));

                var accountsDto = accounts.Select(x => new AccountsPayableListResponse
                {
                    Id = x.Id,
                    AmountOwed = x.AmountOwed,
                    DueDate = x.DueDate,
                    AmountPaid = x.AmountPaid,
                    BalanceOwed = x.BalanceOwed,
                    AccountsPayableStatus = x.AccountsPayableStatus.Name,
                    SupplierId = x.SupplierId,
                    Supplier = x.Supplier.Name
                }).ToList();

                return ApiResponse<List<AccountsPayableListResponse>>.CreateSuccess(accountsDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving accounts payable. {ex.Message}", ex);

                return ApiResponse<List<AccountsPayableListResponse>>.CreateFailure($"An error occurred while retrieving accounts payable. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<AccountsPayableDetailsResponse>> GetAccountPayable(int id)
        {
            try
            {
                var account = await _unitOfWork.Repository<AccountsPayable>().GetByIdWithIncludesAsync(id, query =>
                    query.Include(s => s.Supplier)
                         .Include(a => a.AccountsPayableStatus));

                if (account == null)
                    return ApiResponse<AccountsPayableDetailsResponse>.CreateFailure("Account not found", 404);

                var accountDto = new AccountsPayableDetailsResponse
                {
                    AmountOwed = account.AmountOwed,
                    DueDate = account.DueDate,
                    AmountPaid = account.AmountPaid,
                    BalanceOwed = account.BalanceOwed,
                    AccountsPayableStatus = account.AccountsPayableStatus.Name,
                    AccountsPayableStatusId = account.AccountsPayableStatusId,
                    Supplier = account.Supplier.Name,
                    SupplierId = account.SupplierId
                };

                return ApiResponse<AccountsPayableDetailsResponse>.CreateSuccess(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving account payable. {ex.Message}", ex);

                return ApiResponse<AccountsPayableDetailsResponse>.CreateFailure($"An error occurred while retrieving account payable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<AccountsPayableListResponse>>> GetPastDueAccountsPayable()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        ap.Id,
                        ap.AmountOwed,
                        ap.DueDate,
                        ap.AmountPaid,
                        ap.BalanceOwed,
                        aps.Name AS AccountsPayableStatus,
                        ap.SupplierId,
                        s.Name AS Supplier
                    FROM AccountsPayable ap
                    INNER JOIN AccountsPayableStatus aps ON ap.AccountsPayableStatusId = aps.Id
                    INNER JOIN Suppliers s ON ap.SupplierId = s.Id
                    WHERE ap.DueDate < @CurrentDate AND ap.BalanceOwed > 0
                    ORDER BY ap.DueDate";

                using var conn = _context.CreateConnection();

                var accounts = await conn.QueryAsync<AccountsPayableListResponse>(
                    sql,
                    new { CurrentDate = DateTime.Now.Date }
                );

                return ApiResponse<List<AccountsPayableListResponse>>.CreateSuccess(accounts.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving past due accounts payable. {ex.Message}", ex);

                return ApiResponse<List<AccountsPayableListResponse>>.CreateFailure($"An error occurred while retrieving past due accounts payable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<SupplierAccountSummaryResponse>>> GetSupplierAccountsSummary()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        s.Id AS SupplierId,
                        s.Name AS SupplierName,
                        SUM(ap.AmountOwed) AS TotalAmountOwed,
                        SUM(ap.AmountPaid) AS TotalAmountPaid,
                        SUM(ap.BalanceOwed) AS TotalBalanceOwed,
                        COUNT(ap.Id) AS NumberOfAccounts,
                        MIN(CASE WHEN ap.BalanceOwed > 0 THEN ap.DueDate ELSE NULL END) AS EarliestDueDate
                    FROM Suppliers s
                    LEFT JOIN AccountsPayable ap ON s.Id = ap.SupplierId
                    GROUP BY s.Id, s.Name
                    HAVING SUM(ap.BalanceOwed) > 0
                    ORDER BY SUM(ap.BalanceOwed) DESC";

                using var conn = _context.CreateConnection();

                var summary = await conn.QueryAsync<SupplierAccountSummaryResponse>(sql);

                return ApiResponse<List<SupplierAccountSummaryResponse>>.CreateSuccess(summary.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving supplier accounts summary. {ex.Message}", ex);

                return ApiResponse<List<SupplierAccountSummaryResponse>>.CreateFailure($"An error occurred while retrieving supplier accounts summary. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> AddAccountsPayable(AddAccountsPayableModel request)
        {
            try
            {
                var obj = new AccountsPayable
                {
                    AmountOwed = request.AmountOwed,
                    DueDate = request.DueDate,
                    AmountPaid = request.AmountPaid,
                    BalanceOwed = request.BalanceOwed,
                    AccountsPayableStatusId = request.AccountsPayableStatusId,
                    SupplierId = request.SupplierId
                };

                await _unitOfWork.Repository<AccountsPayable>().AddAsync(obj);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result < 0) return ApiResponse<bool>.CreateFailure("Failed to add account payable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding account payable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while adding account payable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAccountsPayable(UpdateAccountsPayableModel request)
        {
            try
            {
                var existingAccountsPayable = await _unitOfWork.Repository<AccountsPayable>().GetByIdAsync(request.Id);

                if (existingAccountsPayable == null)
                    return ApiResponse<bool>.CreateFailure("Account payable not found", 404);

                existingAccountsPayable.AmountOwed = request.AmountOwed;
                existingAccountsPayable.DueDate = request.DueDate;
                existingAccountsPayable.AmountPaid = request.AmountPaid;
                existingAccountsPayable.BalanceOwed = request.BalanceOwed;
                existingAccountsPayable.AccountsPayableStatusId = request.AccountsPayableStatusId;
                existingAccountsPayable.SupplierId = request.SupplierId;

                _unitOfWork.Repository<AccountsPayable>().Update(existingAccountsPayable);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result < 0)
                    return ApiResponse<bool>.CreateFailure("Failed to update account payable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating account payable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating account payable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAccountsPayable(int id)
        {
            try
            {
                var account = await _unitOfWork.Repository<AccountsPayable>().GetByIdAsync(id);

                if (account == null)
                    return ApiResponse<bool>.CreateFailure("Account payable not found", 404);

                _unitOfWork.Repository<AccountsPayable>().Delete(account);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result < 0)
                    return ApiResponse<bool>.CreateFailure("Failed to delete account payable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting account payable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting account payable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SupplierStatementResponse>> GetSupplierStatement(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to last 12 months if no dates provided
                startDate ??= DateTime.Now.AddMonths(-12);
                endDate ??= DateTime.Now;

                const string sql = @"
                WITH SupplierTransactions AS (
                    -- Purchases (Credits) - amounts we owe
                    SELECT 
                        p.PurchaseDate AS TransactionDate,
                        'Purchase' AS TransactionType,
                        'Purchase Order #' + CAST(p.Id AS VARCHAR) AS Description,
                        0 AS DebitAmount,
                        p.GrandTotal AS CreditAmount,
                        p.Id AS ReferenceId,
                        'Purchase' AS ReferenceTable
                    FROM [Pharma263].Purchase p
                    WHERE p.SupplierId = @SupplierId 
                        AND p.PurchaseDate BETWEEN @StartDate AND @EndDate
                        AND p.IsDeleted = 0

                    UNION ALL

                    -- Payments (Debits) - amounts we paid
                    SELECT 
                        pm.PaymentDate AS TransactionDate,
                        'Payment' AS TransactionType,
                        'Payment via ' + pmt.Name AS Description,
                        pm.AmountPaid AS DebitAmount,
                        0 AS CreditAmount,
                        pm.Id AS ReferenceId,
                        'PaymentMade' AS ReferenceTable
                    FROM [Pharma263].PaymentMade pm
                    INNER JOIN [Pharma263].PaymentMethod pmt ON pm.PaymentMethodId = pmt.Id
                    WHERE pm.SupplierId = @SupplierId 
                        AND pm.PaymentDate BETWEEN @StartDate AND @EndDate
                        AND pm.IsDeleted = 0
                )
                SELECT 
                    TransactionDate,
                    TransactionType,
                    Description,
                    DebitAmount,
                    CreditAmount,
                    ReferenceId,
                    ReferenceTable,
                    -- Running balance calculation (what we owe)
                    SUM(CreditAmount - DebitAmount) OVER (
                        ORDER BY TransactionDate, TransactionType 
                        ROWS UNBOUNDED PRECEDING
                    ) AS RunningBalance
                FROM SupplierTransactions
                ORDER BY TransactionDate DESC, TransactionType";

                using var conn = _context.CreateConnection();

                var transactions = await conn.QueryAsync<SupplierStatementTransactionDto>(
                    sql,
                    new { SupplierId = supplierId, StartDate = startDate, EndDate = endDate }
                );

                // Get supplier info
                var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
                if (supplier == null)
                    return ApiResponse<SupplierStatementResponse>.CreateFailure("Supplier not found", 404);

                // Calculate summary
                var totalDebits = transactions.Sum(t => t.DebitAmount);
                var totalCredits = transactions.Sum(t => t.CreditAmount);
                var currentBalance = totalCredits - totalDebits; // What we owe

                var statement = new SupplierStatementResponse
                {
                    SupplierId = supplierId,
                    SupplierName = supplier.Name,
                    Email = supplier.Email,
                    Phone = supplier.Phone,
                    StatementPeriod = new DateRangeDto
                    {
                        StartDate = startDate.Value,
                        EndDate = endDate.Value
                    },
                    TotalDebits = totalDebits,
                    TotalCredits = totalCredits,
                    CurrentBalance = currentBalance,
                    Transactions = transactions.ToList(),
                    GeneratedDate = DateTime.Now
                };

                return ApiResponse<SupplierStatementResponse>.CreateSuccess(statement);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while generating supplier statement. {ex.Message}", ex);
                return ApiResponse<SupplierStatementResponse>.CreateFailure($"An error occurred while generating supplier statement. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetSupplierStatementPdf(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Get statement data
                var statementResponse = await GetSupplierStatement(supplierId, startDate, endDate);

                if (!statementResponse.Success || statementResponse.Data == null)
                    return null;

                // Get store settings
                var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();
                var storeInfo = store.FirstOrDefault();

                if (storeInfo == null)
                    throw new Exception("Store settings not found");

                // Create view model for PDF generation
                var viewModel = new SupplierStatementViewModel
                {
                    Company = new StoreSettingsDetailsDto
                    {
                        Id = storeInfo.Id,
                        StoreName = storeInfo.StoreName,
                        Address = storeInfo.Address,
                        Phone = storeInfo.Phone,
                        Email = storeInfo.Email,
                        Logo = storeInfo.Logo,
                        BankingDetails = storeInfo.BankingDetails,
                        Currency = storeInfo.Currency,
                        MCAZLicence = storeInfo.MCAZLicence,
                        ReturnsPolicy = storeInfo.ReturnsPolicy,
                        VATNumber = storeInfo.VATNumber
                    },
                    Statement = statementResponse.Data
                };

                // Generate PDF
                var supplierStatementReport = new SupplierStatementReport();
                return supplierStatementReport.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while generating supplier statement PDF. {ex.Message}", ex);
                throw;
            }
        }
    }
}
