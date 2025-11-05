using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Models.AccountsPayable.Request;
using Pharma263.Api.Models.PaymentMade.Request;
using Pharma263.Api.Models.PaymentMade.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMadeController : ControllerBase
    {
        private readonly PaymentMadeService _paymentMadeService;
        private readonly AccountsPayableService _accountsPayableService;

        public PaymentMadeController(PaymentMadeService paymentMadeService, AccountsPayableService accountsPayableService)
        {
            _paymentMadeService = paymentMadeService;
            _accountsPayableService = accountsPayableService;
        }

        [HttpGet("GetPaymentsMade")]
        public async Task<ActionResult<ApiResponse<List<PaymentMadeListResponse>>>> GetPaymentsMade()
        {
            var payments = await _paymentMadeService.GetPaymentsMade();

            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentSummaryBySupplier")]
        public async Task<ActionResult<ApiResponse<List<PaymentMadeListResponse>>>> GetPaymentSummaryBySupplier([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var payments = await _paymentMadeService.GetPaymentSummaryBySupplier(startDate, endDate);

            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentsMadeToSupplier")]
        public async Task<ActionResult<List<PaymentMadeListResponse>>> GetPaymentHistory([FromQuery] int supplierId)
        {
            var payments = await _paymentMadeService.GetPaymentsMadeToSupplier(supplierId);

            return StatusCode(payments.StatusCode, payments);
        }

        [HttpGet("GetPaymentMade/{id}")]
        public async Task<ActionResult<ApiResponse<PaymentMadeDetailsResponse>>> GetPaymentMade(int id)
        {
            var payment = await _paymentMadeService.GetPaymentMade(id);

            return StatusCode(payment.StatusCode, payment);
        }

        [HttpPost("AddPaymentMade")]
        public async Task<ActionResult<ApiResponse<int>>> AddPaymentMade([FromBody] AddPaymentMadeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Begin transaction for payment & account update
            var paymentResult = await _paymentMadeService.AddPaymentMadeWithAccountUpdate(request);

            if (!paymentResult.Success)
                return StatusCode(paymentResult.StatusCode, paymentResult);

            // Get the account to update
            var accountResponse = await _accountsPayableService.GetAccountPayable(request.AccountPayableId);

            if (!accountResponse.Success)
                return StatusCode(accountResponse.StatusCode,
                    ApiResponse<int>.CreateFailure($"Payment created but failed to update account: {accountResponse.Message}", 500));

            // Update the account with the new payment
            var account = accountResponse.Data;
            var updateRequest = new UpdateAccountsPayableModel
            {
                Id = request.AccountPayableId,
                AmountOwed = account.AmountOwed,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid + request.AmountPaid,
                BalanceOwed = account.BalanceOwed - request.AmountPaid,
                AccountsPayableStatusId = account.BalanceOwed - request.AmountPaid <= 0 ? 2 : 1, // Paid or Outstanding
                SupplierId = request.SupplierId
            };

            var updateResult = await _accountsPayableService.UpdateAccountsPayable(updateRequest);

            if (!updateResult.Success)
                return StatusCode(updateResult.StatusCode,
                    ApiResponse<int>.CreateFailure($"Payment created but failed to update account: {updateResult.Message}", 500));

            return Ok(paymentResult);
        }

        [HttpPut("UpdatePaymentMade")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdatePaymentMade([FromBody] UpdatePaymentMadeRequet request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the original payment first
            var originalPayment = await _paymentMadeService.GetPaymentMade(request.Id);

            if (!originalPayment.Success)
                return StatusCode(originalPayment.StatusCode, originalPayment);

            // Calculate the payment difference
            decimal paymentDifference = request.AmountPaid - originalPayment.Data.AmountPaid;

            // Update the payment
            var result = await _paymentMadeService.UpdatePaymentMadeWithAccountUpdate(request);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            // If there's a difference in payment amount, update the account
            if (paymentDifference != 0)
            {
                var accountResponse = await _accountsPayableService.GetAccountPayable(request.AccountPayableId);

                if (!accountResponse.Success)
                    return StatusCode(accountResponse.StatusCode,
                        ApiResponse<bool>.CreateFailure($"Payment updated but failed to update account: {accountResponse.Message}", 500));

                var account = accountResponse.Data;
                var updateRequest = new UpdateAccountsPayableModel
                {
                    Id = request.AccountPayableId,
                    AmountOwed = account.AmountOwed,
                    DueDate = account.DueDate,
                    AmountPaid = account.AmountPaid + paymentDifference,
                    BalanceOwed = account.BalanceOwed - paymentDifference,
                    AccountsPayableStatusId = account.BalanceOwed - paymentDifference <= 0 ? 2 : 1, // Paid or Outstanding
                    SupplierId = originalPayment.Data.SupplierId
                };

                var updateResult = await _accountsPayableService.UpdateAccountsPayable(updateRequest);

                if (!updateResult.Success)
                    return StatusCode(updateResult.StatusCode,
                        ApiResponse<bool>.CreateFailure($"Payment updated but failed to update account: {updateResult.Message}", 500));
            }

            return Ok(result);
        }

        [HttpDelete("DeletePaymentMade/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePaymentMade(int id)
        {
            // Get the original payment first to know how much to adjust the account
            var originalPayment = await _paymentMadeService.GetPaymentMade(id);

            if (!originalPayment.Success)
                return StatusCode(originalPayment.StatusCode, originalPayment);

            // Delete the payment
            var result = await _paymentMadeService.DeletePaymentMadeWithAccountUpdate(id);

            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            // Adjust the account (add back the deleted payment amount)
            var accountResponse = await _accountsPayableService.GetAccountPayable(originalPayment.Data.AccountPayableId);

            if (!accountResponse.Success)
                return StatusCode(accountResponse.StatusCode,
                    ApiResponse<bool>.CreateFailure($"Payment deleted but failed to update account: {accountResponse.Message}", 500));

            var account = accountResponse.Data;
            var updateRequest = new UpdateAccountsPayableModel
            {
                Id = originalPayment.Data.AccountPayableId,
                AmountOwed = account.AmountOwed,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid - originalPayment.Data.AmountPaid,
                BalanceOwed = account.BalanceOwed + originalPayment.Data.AmountPaid,
                AccountsPayableStatusId = 1, // Outstanding (since we deleted a payment)
                SupplierId = originalPayment.Data.SupplierId
            };

            var updateResult = await _accountsPayableService.UpdateAccountsPayable(updateRequest);

            if (!updateResult.Success)
                return StatusCode(updateResult.StatusCode,
                    ApiResponse<bool>.CreateFailure($"Payment deleted but failed to update account: {updateResult.Message}", 500));

            return Ok(result);
        }
    }
}
