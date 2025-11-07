using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IPurchaseStatusService _purchaseStatusService;
        private readonly ISaleStatusService _saleStatusService;
        private readonly ICustomerTypeService _customerTypeService;
        private readonly ISelectionsService _selectionsService;

        public AdminController(ISupplierService supplierService, IPaymentMethodService paymentMethodService,
            IPurchaseStatusService purchaseStatusService, ICustomerTypeService customerTypeService,
            ISaleStatusService saleStatusService, ISelectionsService selectionsService)
        {
            _supplierService = supplierService;
            _paymentMethodService = paymentMethodService;
            _purchaseStatusService = purchaseStatusService;
            _customerTypeService = customerTypeService;
            _saleStatusService = saleStatusService;
            _selectionsService = selectionsService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> QuickAdd()
        {
            await LoadMedicineDropdown();

            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> QuickAdd(QuickAddDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }
        //}

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetCustomerType()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var customerTypes = await _selectionsService.GetCustomerTypes(token);

            return Json(customerTypes);
        }

        public async Task<JsonResult> GetCustomers()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var customers = await _selectionsService.GetCustomersList(token);

            return Json(customers);
        }

        public async Task<JsonResult> GetSuppliers()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var suppliers = await _selectionsService.GetSuppliersList(token);

            return Json(suppliers);
        }

        public async Task<JsonResult> GetMedicines()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var medicines = await _selectionsService.GetMedicinesList(token);

            return Json(medicines);
        }

        public async Task<JsonResult> GetStocks()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var medicines = await _selectionsService.GetStocks(token);

            return Json(medicines);
        }

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetPaymentMethod()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var paymentMethods = await _selectionsService.GetPaymentMethods(token);

            return Json(paymentMethods);
        }

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetPurchaseStatus()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var purchaseStatuses = await _selectionsService.GetPurchaseStatuses(token);

            return Json(purchaseStatuses);
        }

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetSaleStatus()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var saleStatuses = await _selectionsService.GetSaleStatuses(token);

            return Json(saleStatuses);
        }

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetReturnReasons()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var returnReason = await _selectionsService.GetReturnReasons(token);

            return Json(returnReason);
        }

        [ResponseCache(Duration = 3600, VaryByHeader = "Authorization")]
        public async Task<JsonResult> GetReturnDestinations()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var returnReason = await _selectionsService.GetReturnDestinations(token);

            return Json(returnReason);
        }

        private async Task LoadMedicineDropdown()
        {
            var supplierList = await _supplierService.GetAllAsync();

            ViewBag.suppliers = supplierList.Data.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }
    }
}
