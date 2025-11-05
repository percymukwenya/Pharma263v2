using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Pharma263.MVC.DTOs.AccountsReceivable;
using Pharma263.MVC.DTOs.PaymentReceived;
using Pharma263.MVC.Services.IService;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class AccountsReceivableController : Controller
    {
        private readonly IAccountsReceivableService _accountsReceivableService;
        private readonly IPaymentReceivedService _paymentReceivedService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly ILogger<AccountsReceivableController> _logger;

        public AccountsReceivableController(
            IAccountsReceivableService accountsReceivableService,
            IPaymentReceivedService paymentReceivedService,
            ICustomerService customerService,
            IPaymentMethodService paymentMethodService,
            ILogger<AccountsReceivableController> logger)
        {
            _accountsReceivableService = accountsReceivableService;
            _paymentReceivedService = paymentReceivedService;
            _customerService = customerService;
            _paymentMethodService = paymentMethodService;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var accounts = await _accountsReceivableService.GetAccountsReceivable();

            return View(accounts.Data);
        }

        public async Task<ActionResult> Details(int id)
        {
            var account = await _accountsReceivableService.GetAccountReceivable(id);
            if (account.Data == null && account.Success)
            {
                return NotFound();
            }
            return View(account.Data);
        }

        public async Task<ActionResult> Create()
        {
            var customers = await _customerService.GetCustomers();
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.Customers = customers.Data;
            ViewBag.PaymentMethods = paymentMethods;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddAccountReceivableDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountsReceivableService.AddAccountsReceivable(model);
                if (response.Success)
                {
                    TempData["success"] = "Account Receivable created successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }

            var customers = await _customerService.GetCustomers();
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.Customers = customers.Data;
            ViewBag.PaymentMethods = paymentMethods;

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var response = await _accountsReceivableService.GetAccountReceivable(id);

            if (response.Data == null && response.Success)
                return NotFound();

            var account = response.Data;

            var model = new UpdateAccountReceivableDto
            {
                Id = account.Id,
                AmountDue = account.AmountDue,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid,
                BalanceDue = account.BalanceDue,
                CustomerId = account.CustomerId
            };

            // Get customers for dropdown
            var customers = await _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers.Data, "Id", "Name", model.CustomerId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateAccountReceivableDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountsReceivableService.UpdateAccountsReceivable(model);

                if (result.Success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Failed to update account receivable");
            }

            var customers = await _customerService.GetCustomers();
            ViewBag.Customers = new SelectList(customers.Data, "Id", "Name", model.CustomerId);

            return View(model);
        }

        public async Task<ActionResult> CapturePayment(int accountReceivableId)
        {
            // Get the account details
            var response = await _accountsReceivableService.GetAccountReceivable(accountReceivableId);

            if (response == null && response.Success)
                return NotFound();

            var account = response.Data;

            var model = new AddPaymentReceivedDto
            {
                AccountsReceivableId = accountReceivableId,
                CustomerId = account.CustomerId,
                PaymentDate = DateTime.Now,
                CustomerName = account.Customer,
                AmountDue = account.AmountDue,
                AmountPaid = account.AmountPaid,
                BalanceDue = account.BalanceDue,
                // Default the amount to receive to the full balance due
                AmountToReceive = account.BalanceDue
            };

            // Get payment methods for dropdown
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CapturePayment(AddPaymentReceivedDto model)
        {
            _logger.LogInformation("Received payment request: {Id}={Val}, Amount={Amount}, Date={Date}",
            nameof(model.AccountsReceivableId), model.AccountsReceivableId,
            model.AmountToReceive, model.PaymentDate);

            var amountString = Request.Form["AmountToReceive"].ToString();
            decimal amount;

            if (!decimal.TryParse(amountString,
                          NumberStyles.Number,
                          CultureInfo.InvariantCulture,
                          out amount))
            {
                return Json(new { success = false, message = $"Invalid amount format: {amountString}" });
            }

            model.AmountToReceive = amount;

            var result = await _paymentReceivedService.AddPaymentReceived(new AddPaymentReceivedRequest
            {
                AccountsReceivableId = model.AccountsReceivableId,
                CustomerId = model.CustomerId,
                PaymentDate = model.PaymentDate,
                AmountReceived = model.AmountToReceive,
                PaymentMethodId = model.PaymentMethodId
            });

            if (result.Success)
            {
                TempData["success"] = "Payment recorded successfully";
                return Json(new { success = true, redirectUrl = Url.Action("AccountDashboard", "Dashboard") });
            }

            return Json(new { success = false, message = "Failed to record payment" });
        }

        public async Task<ActionResult> PaymentHistory(int customerId)
        {
            var payments = await _paymentReceivedService.GetPaymentsReceivedFromCustomer(customerId);
            return View(payments.Data);
        }
    }
}
