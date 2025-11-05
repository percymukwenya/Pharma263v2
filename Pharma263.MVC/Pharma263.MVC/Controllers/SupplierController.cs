using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly ISelectionsService _selectionsService;

        public SupplierController(ISupplierService supplierService, ISelectionsService selectionsService)
        {
            _supplierService = supplierService;
            _selectionsService = selectionsService;
        }

        public ActionResult Index()
        {
            // Return empty view - data will be loaded via server-side DataTables
            return View();
        }

        [HttpGet]
        public ActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSupplier(CreateSupplierRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _supplierService.CreateSupplier(model);

                if (response.Success)
                {
                    TempData["success"] = "Supplier saved successfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Message;

                    return View(model);
                }
            }

            TempData["error"] = "Failed to save Supplier.";

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditSupplier(int supplierId)
        {
            var response = await _supplierService.GetAsync(supplierId);

            if (response.Success)
            {
                var supplierToUpdate = new UpdateSupplierRequest
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Email = response.Data.Email,
                    Phone = response.Data.Phone,
                    Address = response.Data.Address,
                    Notes = response.Data.Notes,
                    MCAZLicence = response.Data.MCAZLicence,
                    BusinessPartnerNumber = response.Data.BusinessPartnerNumber,
                    VATNumber = response.Data.VATNumber
                };

                return View(supplierToUpdate);
            }
            else
            {
                TempData["error"] = "Error retrieving Supplier.";

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSupplier(UpdateSupplierRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _supplierService.UpdateSupplier(model);

                if (response.Success == true)
                {
                    TempData["success"] = "Supplier saved successfully";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = response.Message;

                    return View(model);
                }
            }
            else
            {
                TempData["error"] = "Failed to save Supplier.";

                return View(model);
            }
        }

        public async Task<ActionResult> DeleteSupplier(int supplierId)
        {
            var response = await _supplierService.DeleteSupplier(supplierId);

            if (response.Success)
            {
                TempData["success"] = "Supplier deleted successfully";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = response.Message;

                return RedirectToAction("Index");
            }
        }

        public async Task<JsonResult> GetSupplier()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var suppliers = await _selectionsService.GetSuppliersList(token);

            return Json(suppliers);
        }

        [HttpPost]
        public async Task<JsonResult> GetSuppliersDataTable([FromBody] DataTableRequest request)
        {
            var response = await _supplierService.GetSuppliersForDataTable(request);
            return Json(response);
        }
    }
}
