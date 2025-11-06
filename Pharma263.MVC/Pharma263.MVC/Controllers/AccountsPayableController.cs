using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharma263.MVC.DTOs.AccountsPayable;
using Pharma263.MVC.DTOs.PaymentMade;
using Pharma263.MVC.Services.IService;
using System;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class AccountsPayableController : BaseController
    {
        private readonly IAccountsPayableService _accountsPayableService;
        private readonly IPaymentMadeService _paymentMadeService;
        private readonly ISupplierService _supplierService;
        private readonly IPaymentMethodService _paymentMethodService;

        public AccountsPayableController(IAccountsPayableService accountsPayableService, IPaymentMadeService paymentMadeService,
            ISupplierService supplierService, IPaymentMethodService paymentMethodService)
        {
            _accountsPayableService = accountsPayableService;
            _paymentMadeService = paymentMadeService;
            _supplierService = supplierService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<ActionResult> Index()
        {
            var accounts = await _accountsPayableService.GetAccountsPayable();

            return View(accounts.Data);
        }

        public async Task<ActionResult> Details(int id)
        {
            var account = await _accountsPayableService.GetAccountPayable(id);

            if (account.Data == null && account.Success)
            {
                return NotFound();
            }

            return View(account.Data);
        }

        public async Task<ActionResult> Create()
        {
            var suppliers = await _supplierService.GetAllAsync();
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.Suppliers = suppliers.Data;
            ViewBag.PaymentMethods = paymentMethods;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddAccountsPayableDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountsPayableService.AddAccountsPayable(model);
                if (response.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Message);
                }
            }

            var suppliers = await _supplierService.GetAllAsync();
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.Suppliers = suppliers.Data;
            ViewBag.PaymentMethods = paymentMethods;
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var reponse = await _accountsPayableService.GetAccountPayable(id);

            if (reponse.Data == null && reponse.Success)
            {
                return NotFound();
            }

            var account = reponse.Data;

            var model = new UpdateAccountsPayableDto
            {
                Id = account.Id,
                AmountOwed = account.AmountOwed,
                DueDate = account.DueDate,
                AmountPaid = account.AmountPaid,
                BalanceOwed = account.BalanceOwed,
                SupplierId = account.SupplierId
            };

            var suppliers = await _supplierService.GetAllAsync();
            ViewBag.Suppliers = new SelectList(suppliers.Data, "Id", "Name", model.SupplierId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateAccountsPayableDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountsPayableService.UpdateAccountsPayable(model);
                if (response.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Message);
                }
            }
            var suppliers = await _supplierService.GetAllAsync();
            ViewBag.Suppliers = new SelectList(suppliers.Data, "Id", "Name", model.SupplierId);
            return View(model);
        }

        public async Task<ActionResult> CapturePayment(int accountPayableId)
        {
            var response = await _accountsPayableService.GetAccountPayable(accountPayableId);
            if (response.Data == null && response.Success)
            {
                return NotFound();
            }

            var account = response.Data;

            var paymentMade = new AddPaymentMadeDto
            {
                AccountPayableId = accountPayableId,
                SupplierId = account.SupplierId,
                PaymentDate = DateTime.Now,
                SupplierName = account.Supplier,
                AmountOwed = account.AmountOwed,
                AmountPaid = account.AmountPaid,
                BalanceOwed = account.BalanceOwed,
                // Default the amount to pay to the full balance owed
                AmountToPay = account.BalanceOwed
            };

            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "Id", "Name");

            return View(paymentMade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CapturePayment([FromBody] AddPaymentMadeDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _paymentMadeService.AddPaymentMade(new AddPaymentMadeRequest
                {
                    AccountPayableId = model.AccountPayableId,
                    SupplierId = model.SupplierId,
                    PaymentDate = model.PaymentDate,
                    AmountPaid = model.AmountToPay,
                    PaymentMethodId = model.PaymentMethodId,
                });

                if (response.Success)
                {
                    TempData["success"] = "Payment recorded successfully";
                    return Json(new { success = true, redirectUrl = Url.Action("AccountDashboard", "Dashboard") });
                }

                return Json(new { success = false, message = "Failed to record payment" });
            }
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            ViewBag.PaymentMethods = paymentMethods;
            return View(model);
        }

        public async Task<ActionResult> PaymentHistory(int supplierId)
        {
            var payments = await _paymentMadeService.GetPaymentsMadeToSupplier(supplierId);

            return View(payments.Data);
        }
    }
}
