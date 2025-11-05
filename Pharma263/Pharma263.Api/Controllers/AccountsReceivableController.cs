using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.AccountsReceivable.Request;
using Pharma263.Api.Models.AccountsReceivable.Response;
using Pharma263.Api.Services;
using Pharma263.Application.Models;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsReceivableController : ControllerBase
    {
        private readonly AccountsReceivableService _accountsReceivableService;
        private readonly DapperContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountsReceivableController> _logger;

        public AccountsReceivableController(AccountsReceivableService accountsReceivableService, DapperContext context, 
            IUnitOfWork unitOfWork, ILogger<AccountsReceivableController> logger)
        {
            _accountsReceivableService = accountsReceivableService;
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("GetAccountsReceivable")]
        public async Task<ActionResult<ApiResponse<List<AccountReceivableListResponse>>>> GetAccountsReceivable()
        {
            var accounts = await _accountsReceivableService.GetAccountsReceivable();

            return Ok(accounts);
        }

        [HttpGet("GetAccountReceivable/{id}")]
        public async Task<ActionResult<ApiResponse<AccountReceivableDetailsResponse>>> GetAccountReceivable(int id)
        {
            var account = await _accountsReceivableService.GetAccountReceivable(id);

            return StatusCode(account.StatusCode, account);
        }

        [HttpGet("GetPastDueAccountsReceivable")]
        public async Task<ActionResult<ApiResponse<ApiResponse<List<AccountReceivableListResponse>>>>> GetPastDueAccountsReceivable()
        {
            var account = await _accountsReceivableService.GetPastDueAccountsReceivable();

            return StatusCode(account.StatusCode, account);
        }

        [HttpGet("GetCustomerAccountsSummary")]
        public async Task<ActionResult<ApiResponse<ApiResponse<List<AccountReceivableListResponse>>>>> GetCustomerAccountsSummary()
        {
            var account = await _accountsReceivableService.GetCustomerAccountsSummary();

            return StatusCode(account.StatusCode, account);
        }

        [HttpGet("GetAccountsReceivableAging")]
        public async Task<ActionResult<ApiResponse<ApiResponse<List<AccountReceivableListResponse>>>>> GetAccountsReceivableAging()
        {
            var account = await _accountsReceivableService.GetAccountsReceivableAging();

            return StatusCode(account.StatusCode, account);
        }

        [HttpPost("AddAccountsReceivable")]
        public async Task<ActionResult<ApiResponse<bool>>> AddAccountsReceivable([FromBody] AddAccountReceivableModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid accounts receivable data", ModelState));
            }

            var result = await _accountsReceivableService.AddAccountsReceivable(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateAccountsReceivable")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateAccountsReceivable([FromBody] UpdateAccountReceivableModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid accounts receivable data", ModelState));
            }

            var result = await _accountsReceivableService.UpdateAccountsReceivable(request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteAccountsReceivable/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAccountsReceivable(int id)
        {
            var result = await _accountsReceivableService.DeleteAccountsReceivable(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetCustomerStatement/{customerId}")]
        public async Task<ActionResult<ApiResponse<CustomerStatementResponse>>> GetCustomerStatement(
        int customerId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
        {
            var statement = await _accountsReceivableService.GetCustomerStatement(customerId, startDate, endDate);
            return StatusCode(statement.StatusCode, statement);
        }

        [HttpGet("GetCustomerStatementPdf/{customerId}")]
        public async Task<IActionResult> GetCustomerStatementPdf(
        int customerId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var pdfBytes = await _accountsReceivableService.GetCustomerStatementPdf(customerId, startDate, endDate);

                if (pdfBytes == null)
                    return NotFound("Customer statement could not be generated");

                // Get customer name for filename
                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
                var customerName = customer?.Name?.Replace(" ", "_") ?? "Customer";
                var fileName = $"Customer_Statement_{customerName}_{DateTime.Now:yyyyMMdd}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error generating customer statement PDF: {ex.Message}", ex);
                return StatusCode(500, "Error generating PDF");
            }
        }
    }
}
