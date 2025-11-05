using Dapper;
using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models.PaymentMade.Request;
using Pharma263.Api.Models.PaymentMade.Response;
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
    public class PaymentMadeService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<PaymentMadeService> _logger;
        private readonly DapperContext _context;
        private readonly AccountsPayableService _accountsPayableService;

        public PaymentMadeService(IUnitOfWork unitOfWork, IAppLogger<PaymentMadeService> logger, DapperContext context, AccountsPayableService accountsPayableService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _context = context;
            _accountsPayableService = accountsPayableService;
        }

        public async Task<ApiResponse<List<PaymentMadeListResponse>>> GetPaymentsMade()
        {
            try
            {
                var payments = await _unitOfWork.Repository<PaymentMade>().GetAllAsync(q => q.Include(x => x.PaymentMethod).Include(x => x.Supplier));

                var paymentsDto = payments.Select(x => new PaymentMadeListResponse
                {
                    Id = x.Id,
                    AmountPaid = x.AmountPaid,
                    PaymentDate = x.PaymentDate,
                    PaymentMethod = x.PaymentMethod.Name,
                    AccountPayableId = x.AccountPayableId,
                    SupplierId = x.SupplierId,
                    Supplier = x.Supplier.Name
                }).ToList();

                return ApiResponse<List<PaymentMadeListResponse>>.CreateSuccess(paymentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payments made. {ex.Message}", ex);

                return ApiResponse<List<PaymentMadeListResponse>>.CreateFailure($"An error occurred while retrieving payments made. {ex.Message}", 500);
            }            
        }

        public async Task<ApiResponse<List<PaymentMadeListResponse>>> GetPaymentsMadeToSupplier(int id)
        {
            try
            {
                var payments = await _unitOfWork.Repository<PaymentMade>().FindAsync(x => x.SupplierId == id, query => query.Include(p => p.PaymentMethod).Include(x => x.Supplier));

                var paymentsDto = payments.Select(x => new PaymentMadeListResponse
                {
                    Id = x.Id,
                    AmountPaid = x.AmountPaid,
                    PaymentDate = x.PaymentDate,
                    PaymentMethod = x.PaymentMethod.Name,
                    AccountPayableId = x.AccountPayableId,
                    SupplierId = x.SupplierId,
                    Supplier = x.Supplier.Name
                }).ToList();

                return ApiResponse<List<PaymentMadeListResponse>>.CreateSuccess(paymentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payments made to supplier. {ex.Message}", ex);

                return ApiResponse<List<PaymentMadeListResponse>>.CreateFailure($"An error occurred while retrieving payments made to supplier. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PaymentMadeDetailsResponse>> GetPaymentMade(int id)
        {
            try
            {
                var payment = await _unitOfWork.Repository<PaymentMade>().FirstOrDefaultAsync(x => x.Id == id, q => q.Include(p => p.PaymentMethod).Include(x => x.Supplier));

                if (payment == null)
                    return ApiResponse<PaymentMadeDetailsResponse>.CreateFailure("Payment not found", 404);

                var paymentDto = new PaymentMadeDetailsResponse
                {
                    Id = payment.Id,
                    AmountPaid = payment.AmountPaid,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod.Name,
                    AccountPayableId = payment.AccountPayableId,
                    SupplierId = payment.SupplierId,
                    SupplierName = payment.Supplier.Name
                };

                return ApiResponse<PaymentMadeDetailsResponse>.CreateSuccess(paymentDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payment made. {ex.Message}", ex);

                return ApiResponse<PaymentMadeDetailsResponse>.CreateFailure($"An error occurred while retrieving payment made. {ex.Message}", 500);
            }            
        }

        public async Task<ApiResponse<int>> AddPaymentMade(AddPaymentMadeRequest request)
        {
            try
            {
                var paymentToCreate = new PaymentMade
                {
                    AmountPaid = request.AmountPaid,
                    PaymentDate = request.PaymentDate,
                    PaymentMethodId = request.PaymentMethodId,
                    AccountPayableId = request.AccountPayableId,
                    SupplierId = request.SupplierId,
                };

                await _unitOfWork.Repository<PaymentMade>().AddAsync(paymentToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<int>.CreateFailure("Failed to create payment made", 500);


                return ApiResponse<int>.CreateSuccess(paymentToCreate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding payment made. {ex.Message}", ex);

                return ApiResponse<int>.CreateFailure($"An error occurred while adding payment made. {ex.Message}", 500);
            }            
        }

        public async Task<ApiResponse<int>> AddPaymentMadeWithAccountUpdate(AddPaymentMadeRequest request)
        {
            try
            {
                // Execute the entire operation within a transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // 1. Add the payment
                    var paymentToCreate = new PaymentMade
                    {
                        AmountPaid = request.AmountPaid,
                        PaymentDate = request.PaymentDate,
                        PaymentMethodId = request.PaymentMethodId,
                        AccountPayableId = request.AccountPayableId,
                        SupplierId = request.SupplierId,
                    };

                    await _unitOfWork.Repository<PaymentMade>().AddAsync(paymentToCreate);
                    await _unitOfWork.SaveChangesAsync();

                    // 2. Get the account to update
                    var accountPayable = await _unitOfWork.Repository<AccountsPayable>()
                        .GetByIdWithIncludesAsync(request.AccountPayableId, query =>
                            query.Include(x => x.AccountsPayableStatus)
                                 .Include(x => x.Supplier));

                    if (accountPayable == null)
                        throw new Exception($"Account payable with ID {request.AccountPayableId} not found");

                    // 3. Update the account with new payment
                    accountPayable.AmountPaid += request.AmountPaid;
                    accountPayable.BalanceOwed -= request.AmountPaid;

                    // Update status if fully paid
                    accountPayable.AccountsPayableStatusId = accountPayable.BalanceOwed <= 0 ? 2 : 1; // 2 = Paid, 1 = Outstanding

                    _unitOfWork.Repository<AccountsPayable>().Update(accountPayable);
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

        public async Task<ApiResponse<List<PaymentSummaryBySupplierResponse>>> GetPaymentSummaryBySupplier(DateTime startDate, DateTime endDate)
        {
            try
            {
                const string sql = @"
                    SELECT 
                        s.Id AS SupplierId,
                        s.[Name] AS SupplierName,
                        COUNT(pm.Id) AS PaymentCount,
                        SUM(pm.AmountPaid) AS TotalAmountPaid,
                        MIN(pm.PaymentDate) AS FirstPaymentDate,
                        MAX(pm.PaymentDate) AS LastPaymentDate
                    FROM [Pharma263].Supplier s
                    INNER JOIN [Pharma263].PaymentMade pm ON s.Id = pm.SupplierId
                    WHERE pm.PaymentDate BETWEEN @StartDate AND @EndDate
                    GROUP BY s.Id, s.Name
                    ORDER BY SUM(pm.AmountPaid) DESC";

                using var conn = _context.CreateConnection();

                var summary = await conn.QueryAsync<PaymentSummaryBySupplierResponse>(
                    sql,
                    new { StartDate = startDate, EndDate = endDate }
                );

                return ApiResponse<List<PaymentSummaryBySupplierResponse>>.CreateSuccess(summary.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving payment summary by supplier. {ex.Message}", ex);
                return ApiResponse<List<PaymentSummaryBySupplierResponse>>.CreateFailure($"An error occurred while retrieving payment summary by supplier. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePaymentMade(UpdatePaymentMadeRequet request)
        {
            try
            {
                var existingPaymentMade = await _unitOfWork.Repository<PaymentMade>().GetByIdAsync(request.Id);

                if (existingPaymentMade == null)
                    return ApiResponse<bool>.CreateFailure("Payment made not found", 404);

                existingPaymentMade.AmountPaid = request.AmountPaid;
                existingPaymentMade.PaymentDate = request.PaymentDate;
                existingPaymentMade.PaymentMethodId = request.PaymentMethodId;
                existingPaymentMade.AccountPayableId = request.AccountPayableId;

                await _unitOfWork.Repository<PaymentMade>().AddAsync(existingPaymentMade);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Failed to update payment made", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating payment made. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating payment made. {ex.Message}", 500);
            }            
        }

        public async Task<ApiResponse<bool>> UpdatePaymentMadeWithAccountUpdate(UpdatePaymentMadeRequet request)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Get the original payment to calculate the difference
                    var originalPayment = await _unitOfWork.Repository<PaymentMade>().GetByIdAsync(request.Id);

                    if (originalPayment == null)
                        throw new Exception($"Payment with ID {request.Id} not found");

                    decimal paymentDifference = request.AmountPaid - originalPayment.AmountPaid;

                    // Only proceed with account update if payment amount changed
                    if (paymentDifference != 0)
                    {
                        // Get account
                        var accountPayable = await _unitOfWork.Repository<AccountsPayable>().GetByIdAsync(request.AccountPayableId);

                        if (accountPayable == null)
                            throw new Exception($"Account payable with ID {request.AccountPayableId} not found");

                        // Update account with payment difference
                        accountPayable.AmountPaid += paymentDifference;
                        accountPayable.BalanceOwed -= paymentDifference;
                        accountPayable.AccountsPayableStatusId = accountPayable.BalanceOwed <= 0 ? 2 : 1;

                        _unitOfWork.Repository<AccountsPayable>().Update(accountPayable);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    // Update payment details
                    originalPayment.AmountPaid = request.AmountPaid;
                    originalPayment.PaymentDate = request.PaymentDate;
                    originalPayment.PaymentMethodId = request.PaymentMethodId;
                    originalPayment.AccountPayableId = request.AccountPayableId;

                    _unitOfWork.Repository<PaymentMade>().Update(originalPayment);
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

        public async Task<ApiResponse<bool>> DeletePaymentMade(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<PaymentMade>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Payment made not found", 404);

                await _unitOfWork.Repository<PaymentMade>().AddAsync(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Failed to delete payment made", 500);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting payment made. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting payment made. {ex.Message}", 500);
            }            
        }

        public async Task<ApiResponse<bool>> DeletePaymentMadeWithAccountUpdate(int id)
        {
            try
            {
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Get the payment to be deleted
                    var paymentToDelete = await _unitOfWork.Repository<PaymentMade>().GetByIdAsync(id);

                    if (paymentToDelete == null)
                        throw new Exception($"Payment with ID {id} not found");

                    // Get the associated account
                    var accountPayable = await _unitOfWork.Repository<AccountsPayable>().GetByIdAsync(paymentToDelete.AccountPayableId);

                    if (accountPayable == null)
                        throw new Exception($"Account payable with ID {paymentToDelete.AccountPayableId} not found");

                    // Adjust account balance by reversing the payment
                    accountPayable.AmountPaid -= paymentToDelete.AmountPaid;
                    accountPayable.BalanceOwed += paymentToDelete.AmountPaid;
                    accountPayable.AccountsPayableStatusId = 1; // Set back to Outstanding

                    _unitOfWork.Repository<AccountsPayable>().Update(accountPayable);

                    // Delete the payment
                    _unitOfWork.Repository<PaymentMade>().Delete(paymentToDelete);

                    await _unitOfWork.SaveChangesAsync();

                    return ApiResponse<bool>.CreateSuccess(true);
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting payment with account update. {ex.Message}", ex);
                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting payment with account update. {ex.Message}", 500);
            }
        }
    }
}
