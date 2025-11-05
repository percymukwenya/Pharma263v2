using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Models.AccountsReceivable.Request;
using Pharma263.Api.Models.PaymentReceived.Request;
using Pharma263.Api.Models.PaymentReceived.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentReceivedController : ControllerBase
    {
        private readonly PaymentReceivedService _paymentReceivedService;
        private readonly AccountsReceivableService _accountsReceivableService;

        public PaymentReceivedController(PaymentReceivedService paymentReceivedService, AccountsReceivableService accountsReceivableService)
        {
            _paymentReceivedService = paymentReceivedService;
            _accountsReceivableService = accountsReceivableService;
        }

        [HttpGet("GetPaymentHistory")]
        public async Task<ActionResult<List<PaymentReceivedListResponse>>> GetPaymentHistory([FromQuery] int customerId)
        {
            var payments = await _paymentReceivedService.GetPaymentsReceivedFromCustomer(customerId);

            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentsReceived")]
        public async Task<ActionResult<ApiResponse<List<PaymentReceivedListResponse>>>> GetPaymentsReceived()
        {
            var payments = await _paymentReceivedService.GetPaymentsReceived();
            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentReceived/{id}")]
        public async Task<ActionResult<ApiResponse<PaymentReceivedDetailsResponse>>> GetPaymentReceived(int id)
        {
            var payment = await _paymentReceivedService.GetPaymentReceived(id);

            if (!payment.Success)
                return StatusCode(payment.StatusCode, payment);

            return Ok(payment);
        }

        [HttpGet("GetPaymentSummaryByCustomer")]
        public async Task<ActionResult<ApiResponse<List<PaymentReceivedListResponse>>>> GetPaymentSummaryByCustomer(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var payments = await _paymentReceivedService.GetPaymentSummaryByCustomer(startDate, endDate);
            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentsReceivedFromCustomer/{customerId:int}")]
        public async Task<ActionResult<ApiResponse<PaymentReceivedDetailsResponse>>> GetPaymentsReceivedFromCustomer(int customerId)
        {
            var payment = await _paymentReceivedService.GetPaymentsReceivedFromCustomer(customerId);

            if (!payment.Success)
                return StatusCode(payment.StatusCode, payment);

            return Ok(payment);
        }

        [HttpPost("AddPaymentReceived")]
        public async Task<ActionResult<ApiResponse<int>>> AddPaymentReceived([FromBody] AddPaymentReceivedRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Add the payment
            var paymentResult = await _paymentReceivedService.AddPaymentReceived(request);

            if (!paymentResult.Success)
                return StatusCode(paymentResult.StatusCode, paymentResult);

            // Get the account to update
            var accountResponse = await _accountsReceivableService.GetAccountReceivable(request.AccountsReceivableId);

            if (!accountResponse.Success)
                return StatusCode(accountResponse.StatusCode,
                    ApiResponse<int>.CreateFailure($"Payment created but failed to update account: {accountResponse.Message}", 500));

            // Update the account with the new payment
            var account = accountResponse.Data;
            var updateRequest = new UpdateAccountReceivableModel
            {
                Id = request.AccountsReceivableId,
                AmountDue = account.AmountDue,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid + request.AmountReceived,
                BalanceDue = account.BalanceDue - request.AmountReceived,
                AccountsReceivableStatusId = account.BalanceDue - request.AmountReceived <= 0 ? 2 : 1, // Paid or Outstanding
                CustomerId = request.CustomerId
            };

            var updateResult = await _accountsReceivableService.UpdateAccountsReceivable(updateRequest);

            if (!updateResult.Success)
                return StatusCode(updateResult.StatusCode,
                    ApiResponse<int>.CreateFailure($"Payment created but failed to update account: {updateResult.Message}", 500));

            return Ok(paymentResult);
        }

        [HttpPut("UpdatePaymentReceived")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdatePaymentReceived([FromBody] UpdatePaymentReceivedRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the original payment first
            var originalPayment = await _paymentReceivedService.GetPaymentReceived(request.Id);

            if (!originalPayment.Success)
                return StatusCode(originalPayment.StatusCode, originalPayment);

            // Calculate the payment difference
            decimal paymentDifference = request.AmountReceived - originalPayment.Data.AmountReceived;

            // Update the payment
            var result = await _paymentReceivedService.UpdatePaymentReceived(request);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            // If there's a difference in payment amount, update the account
            if (paymentDifference != 0)
            {
                var accountResponse = await _accountsReceivableService.GetAccountReceivable(request.AccountsReceivableId);

                if (!accountResponse.Success)
                    return StatusCode(accountResponse.StatusCode,
                        ApiResponse<bool>.CreateFailure($"Payment updated but failed to update account: {accountResponse.Message}", 500));

                var account = accountResponse.Data;
                var updateRequest = new UpdateAccountReceivableModel
                {
                    Id = request.AccountsReceivableId,
                    AmountDue = account.AmountDue,
                    DueDate = account.DueDate,
                    AmountPaid = account.AmountPaid + paymentDifference,
                    BalanceDue = account.BalanceDue - paymentDifference,
                    AccountsReceivableStatusId = account.BalanceDue - paymentDifference <= 0 ? 2 : 1, // Paid or Outstanding
                    CustomerId = request.CustomerId
                };

                var updateResult = await _accountsReceivableService.UpdateAccountsReceivable(updateRequest);

                if (!updateResult.Success)
                    return StatusCode(updateResult.StatusCode,
                        ApiResponse<bool>.CreateFailure($"Payment updated but failed to update account: {updateResult.Message}", 500));
            }

            return Ok(result);
        }

        [HttpDelete("DeletePaymentReceived/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePaymentReceived(int id)
        {
            // Get the original payment first to know how much to adjust the account
            var originalPayment = await _paymentReceivedService.GetPaymentReceived(id);

            if (!originalPayment.Success)
                return StatusCode(originalPayment.StatusCode, originalPayment);

            // Delete the payment
            var result = await _paymentReceivedService.DeletePaymentReceived(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            // Adjust the account (add back the deleted payment amount)
            var accountResponse = await _accountsReceivableService.GetAccountReceivable(originalPayment.Data.AccountsReceivableId);

            if (!accountResponse.Success)
                return StatusCode(accountResponse.StatusCode,
                    ApiResponse<bool>.CreateFailure($"Payment deleted but failed to update account: {accountResponse.Message}", 500));

            var account = accountResponse.Data;
            var updateRequest = new UpdateAccountReceivableModel
            {
                Id = originalPayment.Data.AccountsReceivableId,
                AmountDue = account.AmountDue,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid - originalPayment.Data.AmountReceived,
                BalanceDue = account.BalanceDue + originalPayment.Data.AmountReceived,
                AccountsReceivableStatusId = 1, // Outstanding (since we deleted a payment)
                CustomerId = originalPayment.Data.CustomerId
            };

            var updateResult = await _accountsReceivableService.UpdateAccountsReceivable(updateRequest);

            if (!updateResult.Success)
                return StatusCode(updateResult.StatusCode,
                    ApiResponse<bool>.CreateFailure($"Payment deleted but failed to update account: {updateResult.Message}", 500));

            return Ok(result);
        }
    }
}
