using Dapper;
using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models.PaymentReceived.Request;
using Pharma263.Api.Models.PaymentReceived.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class PaymentReceivedService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<PaymentReceivedService> _logger;
        private readonly AccountsReceivableService _accountsReceivableService;
        private readonly DapperContext _context;

        public PaymentReceivedService(IUnitOfWork unitOfWork, IAppLogger<PaymentReceivedService> logger, 
            AccountsReceivableService accountsReceivableService, DapperContext context)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _accountsReceivableService = accountsReceivableService;
            _context = context;
        }

        public async Task<ApiResponse<List<PaymentReceivedListResponse>>> GetPaymentsReceived()
        {
            try
            {
                var payments = await _unitOfWork.Repository<PaymentReceived>().GetAllAsync(q => q.Include(x => x.PaymentMethod).Include(x => x.Customer));

                var paymentsDto = payments.Select(x => new PaymentReceivedListResponse
                {
                    Id = x.Id,
                    AmountReceived = x.AmountReceived,
                    PaymentDate = x.PaymentDate,
                    PaymentMethod = x.PaymentMethod.Name,
                    AccountsReceivableId = x.AccountsReceivableId,
                    CustomerId = x.CustomerId,
                    Customer = x.Customer.Name
                }).ToList();

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateSuccess(paymentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payments received. {ex.Message}", ex);

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateFailure($"An error occurred while retrieving payments received. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<PaymentReceivedListResponse>>> GetDapperPaymentsReceived()
        {
            try
            {
                // Use Dapper for better performance with direct SQL
                const string sql = @"
                    SELECT 
                        pr.Id,
                        pr.AmountReceived,
                        pr.PaymentDate,
                        pm.Name AS PaymentMethod,
                        pr.AccountsReceivableId,
                        pr.CustomerId,
                        c.Name AS Customer
                    FROM PaymentReceived pr
                    INNER JOIN PaymentMethods pm ON pr.PaymentMethodId = pm.Id
                    INNER JOIN Customers c ON pr.CustomerId = c.Id
                    ORDER BY pr.PaymentDate DESC";

                using var conn = _context.CreateConnection();

                var payments = await conn.QueryAsync<PaymentReceivedListResponse>(sql);

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateSuccess(payments.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payments received. {ex.Message}", ex);

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateFailure($"An error occurred while retrieving payments received. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<PaymentSummaryByCustomerResponse>>> GetPaymentSummaryByCustomer(DateTime startDate, DateTime endDate)
        {
            try
            {
                const string sql = @"
                    SELECT 
                        c.Id AS CustomerId,
                        c.Name AS CustomerName,
                        COUNT(pr.Id) AS PaymentCount,
                        SUM(pr.AmountReceived) AS TotalAmountReceived,
                        MIN(pr.PaymentDate) AS FirstPaymentDate,
                        MAX(pr.PaymentDate) AS LastPaymentDate
                    FROM [Pharma263].[Customer] c
                    INNER JOIN [Pharma263].PaymentReceived pr ON c.Id = pr.CustomerId
                    WHERE pr.PaymentDate BETWEEN @StartDate AND @EndDate
                    GROUP BY c.Id, c.Name
                    ORDER BY SUM(pr.AmountReceived) DESC";

                using var conn = _context.CreateConnection();

                var summary = await conn.QueryAsync<PaymentSummaryByCustomerResponse>(
                    sql,
                    new { StartDate = startDate, EndDate = endDate }
                );

                return ApiResponse<List<PaymentSummaryByCustomerResponse>>.CreateSuccess(summary.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payment summary by customer. {ex.Message}", ex);
                return ApiResponse<List<PaymentSummaryByCustomerResponse>>.CreateFailure($"An error occurred while retrieving payment summary by customer. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<PaymentReceivedListResponse>>> GetPaymentsReceivedFromCustomer(int id)
        {
            try
            {
                var payments = await _unitOfWork.Repository<PaymentReceived>().FindAsync(x => x.CustomerId == id, q => q.Include(x => x.PaymentMethod).Include(x => x.Customer));

                var paymentsDto = payments.Select(x => new PaymentReceivedListResponse
                {
                    Id = x.Id,
                    AmountReceived = x.AmountReceived,
                    PaymentDate = x.PaymentDate,
                    PaymentMethod = x.PaymentMethod.Name,
                    AccountsReceivableId = x.AccountsReceivableId,
                    CustomerId = x.CustomerId,
                    Customer = x.Customer.Name
                }).ToList();

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateSuccess(paymentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payments received from customer. {ex.Message}", ex);

                return ApiResponse<List<PaymentReceivedListResponse>>.CreateFailure($"An error occurred while retrieving payments received from customer. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<PaymentReceivedDetailsResponse>> GetPaymentReceived(int id)
        {
            try
            {
                var payment = await _unitOfWork.Repository<PaymentReceived>().FirstOrDefaultAsync(x => x.Id == id, q => q.Include(x => x.PaymentMethod).Include(x => x.Customer));

                if (payment == null)
                    return ApiResponse<PaymentReceivedDetailsResponse>.CreateFailure("Payment not found", 404);

                var paymentDto = new PaymentReceivedDetailsResponse
                {
                    Id = id,
                    AmountReceived = payment.AmountReceived,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod.Name,
                    AccountsReceivableId = payment.AccountsReceivableId,
                    Customer = payment.Customer.Name,
                };

                return ApiResponse<PaymentReceivedDetailsResponse>.CreateSuccess(paymentDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payment received. {ex.Message}", ex);

                return ApiResponse<PaymentReceivedDetailsResponse>.CreateFailure($"An error occurred while retrieving payment received. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<int>> AddPaymentReceived(AddPaymentReceivedRequest request)
        {
            var objToCreate = new PaymentReceived
            {
                AmountReceived = request.AmountReceived,
                PaymentDate = request.PaymentDate,
                PaymentMethodId = request.PaymentMethodId,
                AccountsReceivableId = request.AccountsReceivableId,
                CustomerId = request.CustomerId
            };

            await _unitOfWork.Repository<PaymentReceived>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return ApiResponse<int>.CreateFailure("Error creating Payment Received");

            return ApiResponse<int>.CreateSuccess(objToCreate.Id);
        }

        public async Task<ApiResponse<int>> AddPaymentReceivedWithAccountUpdate(AddPaymentReceivedRequest request)
        {
            try
            {
                // Execute the entire operation within a transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // 1. Add the payment
                    var paymentToCreate = new PaymentReceived
                    {
                        AmountReceived = request.AmountReceived,
                        PaymentDate = request.PaymentDate,
                        PaymentMethodId = request.PaymentMethodId,
                        AccountsReceivableId = request.AccountsReceivableId,
                        CustomerId = request.CustomerId
                    };

                    await _unitOfWork.Repository<PaymentReceived>().AddAsync(paymentToCreate);
                    await _unitOfWork.SaveChangesAsync();

                    // 2. Get the account to update
                    var accountReceivable = await _unitOfWork.Repository<AccountsReceivable>()
                        .GetByIdWithIncludesAsync(request.AccountsReceivableId, query =>
                            query.Include(x => x.AccountsReceivableStatus)
                                 .Include(x => x.Customer));

                    if (accountReceivable == null)
                        throw new Exception($"Account receivable with ID {request.AccountsReceivableId} not found");

                    // 3. Update the account with new payment
                    accountReceivable.AmountPaid += request.AmountReceived;
                    accountReceivable.BalanceDue -= request.AmountReceived;

                    // Update status if fully paid
                    accountReceivable.AccountsReceivableStatusId = accountReceivable.BalanceDue <= 0 ? 2 : 1; // 2 = Paid, 1 = Outstanding

                    _unitOfWork.Repository<AccountsReceivable>().Update(accountReceivable);
                    await _unitOfWork.SaveChangesAsync();

                    // Success - both operations completed in the transaction
                    return ApiResponse<int>.CreateSuccess(paymentToCreate.Id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding payment with account update. {ex.Message}", ex);
                return ApiResponse<int>.CreateFailure($"An error occurred while adding payment with account update. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePaymentReceived(UpdatePaymentReceivedRequest request)
        {
            try
            {
                var existingPaymentReceived = await _unitOfWork.Repository<PaymentReceived>().GetByIdAsync(request.Id);

                if (existingPaymentReceived == null)
                    return ApiResponse<bool>.CreateFailure("Payment Received not found", 404);

                // Map the request to the existing entity
                existingPaymentReceived.AmountReceived = request.AmountReceived;
                existingPaymentReceived.PaymentDate = request.PaymentDate;
                existingPaymentReceived.PaymentMethodId = request.PaymentMethodId;
                existingPaymentReceived.AccountsReceivableId = request.AccountsReceivableId;
                existingPaymentReceived.CustomerId = request.CustomerId;

                _unitOfWork.Repository<PaymentReceived>().Update(existingPaymentReceived);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error updating Payment Received");

                return ApiResponse<bool>.CreateSuccess(true, "Payment Received updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating payment received. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating payment received. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePaymentReceivedWithAccountUpdate(UpdatePaymentReceivedRequest request)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Get the original payment to calculate the difference
                    var originalPayment = await _unitOfWork.Repository<PaymentReceived>().GetByIdAsync(request.Id);

                    if (originalPayment == null)
                        throw new Exception($"Payment with ID {request.Id} not found");

                    decimal paymentDifference = request.AmountReceived - originalPayment.AmountReceived;

                    // Only proceed with account update if payment amount changed
                    if (paymentDifference != 0)
                    {
                        // Get account
                        var accountReceivable = await _unitOfWork.Repository<AccountsReceivable>().GetByIdAsync(request.AccountsReceivableId);

                        if (accountReceivable == null)
                            throw new Exception($"Account receivable with ID {request.AccountsReceivableId} not found");

                        // Update account with payment difference
                        accountReceivable.AmountPaid += paymentDifference;
                        accountReceivable.BalanceDue -= paymentDifference;
                        accountReceivable.AccountsReceivableStatusId = accountReceivable.BalanceDue <= 0 ? 2 : 1;

                        _unitOfWork.Repository<AccountsReceivable>().Update(accountReceivable);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    // Update payment details
                    originalPayment.AmountReceived = request.AmountReceived;
                    originalPayment.PaymentDate = request.PaymentDate;
                    originalPayment.PaymentMethodId = request.PaymentMethodId;
                    originalPayment.AccountsReceivableId = request.AccountsReceivableId;
                    originalPayment.CustomerId = request.CustomerId;

                    _unitOfWork.Repository<PaymentReceived>().Update(originalPayment);
                    await _unitOfWork.SaveChangesAsync();

                    return ApiResponse<bool>.CreateSuccess(true);
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating payment with account update. {ex.Message}", ex);
                return ApiResponse<bool>.CreateFailure($"An error occurred while updating payment with account update. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeletePaymentReceived(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<PaymentReceived>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Payment Received not found", 404);

                _unitOfWork.Repository<PaymentReceived>().Delete(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error deleting Payment Received");

                return ApiResponse<bool>.CreateSuccess(true, "Payment Received deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting payment received. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting payment received. {ex.Message}", 500);
            }
        }
    }
}
