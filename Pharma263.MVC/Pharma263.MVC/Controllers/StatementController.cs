using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.Services.IService;
using System.Threading.Tasks;
using System;

namespace Pharma263.MVC.Controllers
{
    public class StatementController : BaseController
    {
        private readonly IStatementService _statementService;
        private readonly IAccountsReceivableService _accountsReceivableService;
        private readonly IAccountsPayableService _accountsPayableService;

        public StatementController(
            IStatementService statementService,
            IAccountsReceivableService accountsReceivableService,
            IAccountsPayableService accountsPayableService)
        {
            _statementService = statementService;
            _accountsReceivableService = accountsReceivableService;
            _accountsPayableService = accountsPayableService;
        }

        [HttpGet]
        public async Task<IActionResult> CustomerStatement(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to last 6 months if no date range specified
                startDate ??= DateTime.Now.AddMonths(-6);
                endDate ??= DateTime.Now;

                var statement = await _statementService.GetCustomerStatementAsync(customerId, startDate, endDate);

                if (statement.Data == null)
                {
                    TempData["ErrorMessage"] = "Customer statement could not be generated.";
                    return RedirectToAction("Index", "AccountsReceivable");
                }

                // Set ViewBag data for date range selector
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                ViewBag.CustomerId = customerId;

                return View(statement.Data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error generating statement: {ex.Message}";
                return RedirectToAction("Index", "AccountsReceivable");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SupplierStatement(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Default to last 6 months if no date range specified
                startDate ??= DateTime.Now.AddMonths(-6);
                endDate ??= DateTime.Now;

                var statement = await _statementService.GetSupplierStatementAsync(supplierId, startDate, endDate);

                if (statement.Data == null)
                {
                    TempData["ErrorMessage"] = "Supplier statement could not be generated.";
                    return RedirectToAction("Index", "AccountsPayable");
                }

                // Set ViewBag data for date range selector
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                ViewBag.SupplierId = supplierId;

                return View(statement.Data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error generating statement: {ex.Message}";
                return RedirectToAction("Index", "AccountsPayable");
            }
        }

        // AJAX action for refreshing statement with new date range
        [HttpPost]
        public async Task<IActionResult> RefreshCustomerStatement(int customerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var statement = await _statementService.GetCustomerStatementAsync(customerId, startDate, endDate);
                return PartialView("_CustomerStatementPartial", statement);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RefreshSupplierStatement(int supplierId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var statement = await _statementService.GetSupplierStatementAsync(supplierId, startDate, endDate);
                return PartialView("_SupplierStatementPartial", statement);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadCustomerStatementPdf(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var pdfBytes = await _statementService.GetCustomerStatementPdfAsync(customerId, startDate, endDate);

                if (pdfBytes == null)
                {
                    TempData["ErrorMessage"] = "Customer statement PDF could not be generated.";
                    return RedirectToAction(nameof(CustomerStatement), new { customerId });
                }

                var fileName = $"Customer_Statement_{DateTime.Now:yyyyMMdd}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error generating PDF: {ex.Message}";
                return RedirectToAction(nameof(CustomerStatement), new { customerId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadSupplierStatementPdf(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // This would call your API endpoint that returns PDF
                // For now, we'll redirect to the regular statement view
                return RedirectToAction(nameof(SupplierStatement), new { supplierId, startDate, endDate });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error generating PDF: {ex.Message}";
                return RedirectToAction(nameof(SupplierStatement), new { supplierId });
            }
        }
    }
}
