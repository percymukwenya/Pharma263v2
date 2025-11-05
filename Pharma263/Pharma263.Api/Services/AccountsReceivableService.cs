using Dapper;
using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models;
using Pharma263.Api.Models.AccountsReceivable.Request;
using Pharma263.Api.Models.AccountsReceivable.Response;
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
    public class AccountsReceivableService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<AccountsReceivableService> _logger;
        private readonly DapperContext _context;

        public AccountsReceivableService(IUnitOfWork unitOfWork, IAppLogger<AccountsReceivableService> logger, DapperContext context)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _context = context;
        }

        public async Task<ApiResponse<bool>> AddAccountsReceivable(AddAccountReceivableModel request)
        {
            try
            {
                var obj = new AccountsReceivable
                {
                    AmountDue = request.AmountDue,
                    DueDate = request.DueDate,
                    AmountPaid = request.AmountPaid,
                    BalanceDue = request.BalanceDue,
                    AccountsReceivableStatusId = request.AccountsReceivableStatusId,
                    CustomerId = request.CustomerId
                };

                await _unitOfWork.Repository<AccountsReceivable>().AddAsync(obj);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Failed to add account receivable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding account receivable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while adding account receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<AccountReceivableListResponse>>> GetAccountsReceivable()
        {
            try
            {
                var receivable = await _unitOfWork.Repository<AccountsReceivable>().GetAllAsync(query => query.Include(c => c.Customer).Include(a => a.AccountsReceivableStatus));

                var receivableDto = receivable.Select(x => new AccountReceivableListResponse
                {
                    Id = x.Id,
                    AmountDue = x.AmountDue,
                    DueDate = x.DueDate,
                    AmountPaid = x.AmountPaid,
                    BalanceDue = x.BalanceDue,
                    AccountsReceivableStatus = x.AccountsReceivableStatus.Name,
                    CustomerId = x.CustomerId,
                    Customer = x.Customer.Name
                }).ToList();

                return ApiResponse<List<AccountReceivableListResponse>>.CreateSuccess(receivableDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving accounts receivable. {ex.Message}", ex);

                return ApiResponse<List<AccountReceivableListResponse>>.CreateFailure($"An error occurred while retrieving accounts receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<AccountReceivableDetailsResponse>> GetAccountReceivable(int id)
        {
            try
            {
                var account = await _unitOfWork.Repository<AccountsReceivable>().FirstOrDefaultAsync(x => x.Id == id, q => q.Include(x => x.Customer).Include(x => x.AccountsReceivableStatus));

                if (account == null)
                    return ApiResponse<AccountReceivableDetailsResponse>.CreateFailure("Account not found", 404);

                var accountDto = new AccountReceivableDetailsResponse
                {
                    AccountsReceivableStatus = account.AccountsReceivableStatus.Name,
                    AmountDue = account.AmountDue,
                    DueDate = account.DueDate,
                    AmountPaid = account.AmountPaid,
                    BalanceDue = account.BalanceDue,
                    Customer = account.Customer.Name,
                    CustomerId = account.CustomerId,
                    Id = account.Id
                };

                return ApiResponse<AccountReceivableDetailsResponse>.CreateSuccess(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving account receivable. {ex.Message}", ex);

                return ApiResponse<AccountReceivableDetailsResponse>.CreateFailure($"An error occurred while retrieving account receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<AccountReceivableListResponse>>> GetPastDueAccountsReceivable()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        ar.Id,
                        ar.AmountDue,
                        ar.DueDate,
                        ar.AmountPaid,
                        ar.BalanceDue,
                        ars.Name AS AccountsReceivableStatus,
                        ar.CustomerId,
                        c.Name AS Customer
                    FROM AccountsReceivable ar
                    INNER JOIN AccountsReceivableStatus ars ON ar.AccountsReceivableStatusId = ars.Id
                    INNER JOIN Customers c ON ar.CustomerId = c.Id
                    WHERE ar.DueDate < @CurrentDate AND ar.BalanceDue > 0
                    ORDER BY ar.DueDate";

                using var conn = _context.CreateConnection();

                var accounts = await conn.QueryAsync<AccountReceivableListResponse>(
                    sql,
                    new { CurrentDate = DateTime.Now.Date }
                );

                return ApiResponse<List<AccountReceivableListResponse>>.CreateSuccess(accounts.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving past due accounts receivable. {ex.Message}", ex);

                return ApiResponse<List<AccountReceivableListResponse>>.CreateFailure($"An error occurred while retrieving past due accounts receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<CustomerAccountSummaryResponse>>> GetCustomerAccountsSummary()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        c.Id AS CustomerId,
                        c.Name AS CustomerName,
                        SUM(ar.AmountDue) AS TotalAmountDue,
                        SUM(ar.AmountPaid) AS TotalAmountPaid,
                        SUM(ar.BalanceDue) AS TotalBalanceDue,
                        COUNT(ar.Id) AS NumberOfAccounts,
                        MIN(CASE WHEN ar.BalanceDue > 0 THEN ar.DueDate ELSE NULL END) AS EarliestDueDate
                    FROM Customers c
                    LEFT JOIN AccountsReceivable ar ON c.Id = ar.CustomerId
                    GROUP BY c.Id, c.Name
                    HAVING SUM(ar.BalanceDue) > 0
                    ORDER BY SUM(ar.BalanceDue) DESC";

                using var conn = _context.CreateConnection();

                var summary = await conn.QueryAsync<CustomerAccountSummaryResponse>(sql);

                return ApiResponse<List<CustomerAccountSummaryResponse>>.CreateSuccess(summary.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving customer accounts summary. {ex.Message}", ex);

                return ApiResponse<List<CustomerAccountSummaryResponse>>.CreateFailure($"An error occurred while retrieving customer accounts summary. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<AccountsReceivableAgingResponse>>> GetAccountsReceivableAging()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        c.Id AS CustomerId,
                        c.Name AS CustomerName,
                        SUM(CASE WHEN DATEDIFF(day, ar.DueDate, GETDATE()) <= 30 THEN ar.BalanceDue ELSE 0 END) AS Current30Days,
                        SUM(CASE WHEN DATEDIFF(day, ar.DueDate, GETDATE()) > 30 AND DATEDIFF(day, ar.DueDate, GETDATE()) <= 60 THEN ar.BalanceDue ELSE 0 END) AS Days31To60,
                        SUM(CASE WHEN DATEDIFF(day, ar.DueDate, GETDATE()) > 60 AND DATEDIFF(day, ar.DueDate, GETDATE()) <= 90 THEN ar.BalanceDue ELSE 0 END) AS Days61To90,
                        SUM(CASE WHEN DATEDIFF(day, ar.DueDate, GETDATE()) > 90 THEN ar.BalanceDue ELSE 0 END) AS DaysOver90,
                        SUM(ar.BalanceDue) AS TotalBalance
                    FROM [Pharma263].Customer c
                    LEFT JOIN [Pharma263].AccountsReceivable ar ON c.Id = ar.CustomerId
                    WHERE ar.BalanceDue > 0
                    GROUP BY c.Id, c.Name
                    ORDER BY c.Name";

                using var conn = _context.CreateConnection();

                var report = await conn.QueryAsync<AccountsReceivableAgingResponse>(sql);

                return ApiResponse<List<AccountsReceivableAgingResponse>>.CreateSuccess(report.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving accounts receivable aging report. {ex.Message}", ex);

                return ApiResponse<List<AccountsReceivableAgingResponse>>.CreateFailure($"An error occurred while retrieving accounts receivable aging report. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAccountsReceivable(UpdateAccountReceivableModel request)
        {
            try
            {
                var existingAccoutReceivable = await _unitOfWork.Repository<AccountsReceivable>().GetByIdAsync(request.Id);

                if (existingAccoutReceivable == null)
                    return ApiResponse<bool>.CreateFailure("Account not found", 404);

                existingAccoutReceivable.AmountDue = request.AmountDue;
                existingAccoutReceivable.DueDate = request.DueDate;
                existingAccoutReceivable.AmountPaid = request.AmountPaid;
                existingAccoutReceivable.BalanceDue = request.BalanceDue;
                existingAccoutReceivable.AccountsReceivableStatusId = request.AccountsReceivableStatusId;
                existingAccoutReceivable.CustomerId = request.CustomerId;

                _unitOfWork.Repository<AccountsReceivable>().Update(existingAccoutReceivable);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Failed to update account receivable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating account receivable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating account receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAccountsReceivable(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<AccountsReceivable>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Account not found", 404);

                _unitOfWork.Repository<AccountsReceivable>().Delete(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0)
                    return ApiResponse<bool>.CreateFailure("Failed to delete account receivable", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting account receivable. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting account receivable. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<CustomerStatementResponse>> GetCustomerStatement(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to last 12 months if no dates provided
                startDate ??= DateTime.Now.AddMonths(-12);
                endDate ??= DateTime.Now;

                const string sql = @"
                WITH CustomerTransactions AS (
                    -- Sales (Debits) - amounts due
                    SELECT 
                        s.SalesDate AS TransactionDate,
                        'Sale' AS TransactionType,
                        'Invoice #' + CAST(s.Id AS VARCHAR) AS Description,
                        s.GrandTotal AS DebitAmount,
                        0 AS CreditAmount,
                        s.Id AS ReferenceId,
                        'Sales' AS ReferenceTable
                    FROM [Pharma263].Sales s
                    WHERE s.CustomerId = @CustomerId 
                        AND s.SalesDate BETWEEN @StartDate AND @EndDate
                        AND s.IsDeleted = 0

                    UNION ALL

                    -- Payments (Credits) - amounts received
                    SELECT 
                        pr.PaymentDate AS TransactionDate,
                        'Payment' AS TransactionType,
                        'Payment via ' + pm.Name AS Description,
                        0 AS DebitAmount,
                        pr.AmountReceived AS CreditAmount,
                        pr.Id AS ReferenceId,
                        'PaymentReceived' AS ReferenceTable
                    FROM [Pharma263].PaymentReceived pr
                    INNER JOIN [Pharma263].PaymentMethod pm ON pr.PaymentMethodId = pm.Id
                    WHERE pr.CustomerId = @CustomerId 
                        AND pr.PaymentDate BETWEEN @StartDate AND @EndDate
                        AND pr.IsDeleted = 0

                    UNION ALL

                    -- Returns (Credits) - amounts credited back
                    SELECT 
                        r.DateReturned AS TransactionDate,
                        'Return' AS TransactionType,
                        'Return - ' + rr.Name AS Description,
                        0 AS DebitAmount,
                        si.Amount AS CreditAmount,  -- Return amount from sale item
                        r.Id AS ReferenceId,
                        'Returns' AS ReferenceTable
                    FROM [Pharma263].Returns r
                    INNER JOIN [Pharma263].ReturnReason rr ON r.ReturnReasonId = rr.Id
                    INNER JOIN [Pharma263].Sales s ON r.SaleId = s.Id
                    INNER JOIN [Pharma263].SalesItems si ON s.Id = si.SaleId
                    WHERE s.CustomerId = @CustomerId 
                        AND r.DateReturned BETWEEN @StartDate AND @EndDate
                        AND r.IsDeleted = 0
                )
                SELECT 
                    TransactionDate,
                    TransactionType,
                    Description,
                    DebitAmount,
                    CreditAmount,
                    ReferenceId,
                    ReferenceTable,
                    -- Running balance calculation
                    SUM(DebitAmount - CreditAmount) OVER (
                        ORDER BY TransactionDate, TransactionType 
                        ROWS UNBOUNDED PRECEDING
                    ) AS RunningBalance
                FROM CustomerTransactions
                ORDER BY TransactionDate DESC, TransactionType";

                using var conn = _context.CreateConnection();

                var transactions = await conn.QueryAsync<CustomerStatementTransactionDto>(
                    sql,
                    new { CustomerId = customerId, StartDate = startDate, EndDate = endDate }
                );

                // Get customer info
                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
                if (customer == null)
                    return ApiResponse<CustomerStatementResponse>.CreateFailure("Customer not found", 404);

                // Calculate summary
                var totalDebits = transactions.Sum(t => t.DebitAmount);
                var totalCredits = transactions.Sum(t => t.CreditAmount);
                var currentBalance = totalDebits - totalCredits;

                var statement = new CustomerStatementResponse
                {
                    CustomerId = customerId,
                    CustomerName = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
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

                return ApiResponse<CustomerStatementResponse>.CreateSuccess(statement);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while generating customer statement. {ex.Message}", ex);
                return ApiResponse<CustomerStatementResponse>.CreateFailure($"An error occurred while generating customer statement. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetCustomerStatementPdf(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Get statement data
                var statementResponse = await GetCustomerStatement(customerId, startDate, endDate);

                if (!statementResponse.Success || statementResponse.Data == null)
                    return null;

                // Get store settings
                var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();
                var storeInfo = store.FirstOrDefault();

                if (storeInfo == null)
                    throw new Exception("Store settings not found");

                // Create view model for PDF generation
                var viewModel = new CustomerStatementViewModel
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
                var customerStatementReport = new CustomerStatementReport();
                return customerStatementReport.CreateReport(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while generating customer statement PDF. {ex.Message}", ex);
                throw;
            }
        }
    }
}
