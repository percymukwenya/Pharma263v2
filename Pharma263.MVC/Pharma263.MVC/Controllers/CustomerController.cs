using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ISelectionsService _selectionsService;

        public CustomerController(ICustomerService customerService, ISelectionsService selectionsService)
        {
            _customerService = customerService;
            _selectionsService = selectionsService;
        }

        public ActionResult CustomerList()
        {
            // Return empty view - data will be loaded via server-side DataTables
            return View();
        }

        [HttpGet]
        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCustomer(CreateCustomerRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _customerService.CreateCustomer(model);

                if (response.Success)
                {
                    TempData["success"] = response.Message;

                    return RedirectToAction(nameof(CustomerList));
                }
                else
                {
                    TempData["error"] = response.Message;
                    return View(model);
                }
            }

            TempData["error"] = "Failed to save Customer.";

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditCustomer(int customerId)
        {
            var response = await _customerService.GetCustomer(customerId);

            if (!response.Success) TempData["error"] = response.Message;

            var customerToUpdate = new UpdateCustomerRequest
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Email = response.Data.Email,
                Phone = response.Data.Phone,
                PhysicalAddress = response.Data.PhysicalAddress,
                DeliveryAddress = response.Data.DeliveryAddress,
                MCAZLicence = response.Data.MCAZLicence,
                HPALicense = response.Data.HPALicense,
                VATNumber = response.Data.VATNumber,
                CustomerTypeId = response.Data.CustomerTypeId
            };

            return View(customerToUpdate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCustomer(UpdateCustomerRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _customerService.UpdateCustomer(model);

            if (response.Success)
            {
                TempData["success"] = response.Message;

                return RedirectToAction("CustomerList");
            }
            else
            {
                TempData["error"] = response.Message;

                return View(model);
            }
        }

        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            var response = await _customerService.DeleteCustomer(customerId);

            if (response.Success)
            {
                TempData["success"] = response.Message;

                return RedirectToAction("CustomerList");
            }
            else
            {
                TempData["error"] = response.Message;

                return RedirectToAction("CustomerList");
            }
        }

        public async Task<JsonResult> GetCustomer()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var customers = await _selectionsService.GetCustomersList(token);

            return Json(customers);
        }

        [HttpPost]
        public async Task<JsonResult> GetCustomersDataTable([FromBody] DataTableRequest request)
        {
            var response = await _customerService.GetCustomersForDataTable(request);
            return Json(response);
        }
    }
}
