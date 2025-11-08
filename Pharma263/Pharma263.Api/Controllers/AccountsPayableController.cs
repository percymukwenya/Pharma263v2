using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.AccountsPayable.Request;
using Pharma263.Api.Models.AccountsPayable.Response;
using Pharma263.Api.Services;
using Pharma263.Application.Models;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsPayableController : ControllerBase
    {
        private readonly AccountsPayableService _accountsPayableService;
        private readonly ILogger<AccountsPayableController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AccountsPayableController(AccountsPayableService accountsPayableService, ILogger<AccountsPayableController> logger, IUnitOfWork unitOfWork)
        {
            _accountsPayableService = accountsPayableService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAccountsPayable")]
        public async Task<ActionResult<ApiResponse<List<AccountsPayableListResponse>>>> GetAccountsPayable()
        {
            var accounts = await _accountsPayableService.GetAccountsPayable();

            return Ok(accounts);
        }

        [HttpGet("GetAccountPayable/{id}")]
        public async Task<ActionResult<ApiResponse<AccountsPayableDetailsResponse>>> GetAccountPayable(int id)
        {
            var account = await _accountsPayableService.GetAccountPayable(id);

            return StatusCode(account.StatusCode, account);
        }

        [HttpGet("GetSupplierAccountsSummary")]
        public async Task<ActionResult<ApiResponse<List<AccountsPayableListResponse>>>> GetSupplierAccountsSummary()
        {
            var accounts = await _accountsPayableService.GetSupplierAccountsSummary();

            return Ok(accounts);
        }

        [HttpGet("GetPastDueAccountsPayable")]
        public async Task<ActionResult<ApiResponse<List<AccountsPayableListResponse>>>> GetPastDueAccountsPayable()
        {
            var accounts = await _accountsPayableService.GetPastDueAccountsPayable();

            return Ok(accounts);
        }

        [HttpPost("AddAccountsPayable")]
        public async Task<ActionResult<ApiResponse<bool>>> AddAccountsPayable([FromBody] AddAccountsPayableModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid accounts payable data", ModelState));
            }

            var result = await _accountsPayableService.AddAccountsPayable(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateAccountsPayable")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateAccountsPayable([FromBody] UpdateAccountsPayableModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid accounts payable data", ModelState));
            }

            var result = await _accountsPayableService.UpdateAccountsPayable(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteAccountsPayable/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAccountsPayable(int id)
        {
            var result = await _accountsPayableService.DeleteAccountsPayable(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetSupplierStatement/{supplierId}")]
        public async Task<ActionResult<ApiResponse<SupplierStatementResponse>>> GetSupplierStatement(
        int supplierId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
        {
            var statement = await _accountsPayableService.GetSupplierStatement(supplierId, startDate, endDate);
            return StatusCode(statement.StatusCode, statement);
        }

        [HttpGet("GetSupplierStatementPdf/{supplierId}")]
        public async Task<IActionResult> GetSupplierStatementPdf(
        int supplierId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var pdfBytes = await _accountsPayableService.GetSupplierStatementPdf(supplierId, startDate, endDate);

                if (pdfBytes == null)
                    return NotFound("Supplier statement could not be generated");

                // Get supplier name for filename
                var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
                var supplierName = supplier?.Name?.Replace(" ", "_") ?? "Supplier";
                var fileName = $"Supplier_Statement_{supplierName}_{DateTime.Now:yyyyMMdd}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error generating supplier statement PDF: {ex.Message}", ex);
                return StatusCode(500, "Error generating PDF");
            }
        }
    }
}
