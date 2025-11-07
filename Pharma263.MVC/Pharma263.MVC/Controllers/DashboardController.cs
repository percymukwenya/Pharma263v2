using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IAccountsReceivableService _accountsReceivableService;
        private readonly IAccountsPayableService _accountsPayableService;
        private readonly IPaymentReceivedService _paymentReceivedService;
        private readonly IPaymentMadeService _paymentMadeService;

        public DashboardController(IDashboardService dashboardService, IAccountsReceivableService accountsReceivableService,
            IAccountsPayableService accountsPayableService, IPaymentReceivedService paymentReceivedService,
            IPaymentMadeService paymentMadeService)
        {
            _dashboardService = dashboardService;
            _accountsReceivableService = accountsReceivableService;
            _accountsPayableService = accountsPayableService;
            _paymentReceivedService = paymentReceivedService;
            _paymentMadeService = paymentMadeService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);
            var response = await _dashboardService.GetDashboardWithTrendsAsync(token, 30);

            if (response != null)
            {
                var model = response;
                return View(model);
            }

            return NotFound();
        }

        public IActionResult AccountDashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountingSummary()
        {
            var receivables = await _accountsReceivableService.GetAccountsReceivable();
            var payables = await _accountsPayableService.GetAccountsPayable();

            var today = DateTime.Now.Date;

            var result = new
            {
                totalAccountsReceivable = receivables.Data.Sum(r => r.BalanceDue),
                totalAccountsPayable = payables.Data.Sum(p => p.BalanceOwed),
                pastDueReceivables = receivables.Data.Where(r => r.DueDate < today && r.BalanceDue > 0).Sum(r => r.BalanceDue),
                pastDuePayables = payables.Data.Where(p => p.DueDate < today && p.BalanceOwed > 0).Sum(p => p.BalanceOwed)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPastDueReceivables()
        {
            var receivables = await _accountsReceivableService.GetAccountsReceivable();

            var today = DateTime.Now.Date;
            var pastDue = receivables.Data
                .Where(r => r.DueDate < today && r.BalanceDue > 0)
                .OrderBy(r => r.DueDate)
                .ToList();

            return Json(pastDue);
        }

        [HttpGet]
        public async Task<IActionResult> GetPastDuePayables()
        {
            var payables = await _accountsPayableService.GetAccountsPayable();

            var today = DateTime.Now.Date;
            var pastDue = payables.Data
                .Where(p => p.DueDate < today && p.BalanceOwed > 0)
                .OrderBy(p => p.DueDate)
                .ToList();

            return Json(pastDue);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountsReceivableAging()
        {
            var aging = await _accountsReceivableService.GetAccountsReceivableAging();
            return Json(aging.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsReceivedChart()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            // Get payments received in the last 30 days
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-30);

            var payments = await _paymentReceivedService.GetPaymentSummaryByCustomer(startDate, endDate);

            // Prepare data for chart
            var result = new
            {
                labels = payments.Data.Select(p => p.CustomerName).ToArray(),
                amounts = payments.Data.Select(p => (double)p.TotalAmountReceived).ToArray()
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsMadeChart()
        {
            // Get payments made in the last 30 days
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-30);

            var payments = await _paymentMadeService.GetPaymentSummaryBySupplier(startDate, endDate);

            // Prepare data for chart
            var result = new
            {
                labels = payments.Data.Select(p => p.SupplierName).ToArray(),
                amounts = payments.Data.Select(p => (double)p.TotalAmountPaid).ToArray()
            };

            return Json(result);
        }
    }
}
