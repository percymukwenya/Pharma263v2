using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class MedicineController : BaseController
    {
        private readonly IMedicineService _medicineService;
        private readonly ISupplierService _supplierService;
        private readonly IStockService _stockService;
        private readonly ISelectionsService _selectionsService;
        private readonly IMapper _mapper;

        public MedicineController(IMedicineService medicineService, ISupplierService supplierService,
            IStockService stockService, ISelectionsService selectionsService, IMapper mapper)
        {
            _medicineService = medicineService;
            _supplierService = supplierService;
            _stockService = stockService;
            _selectionsService = selectionsService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            // Return empty view - data will be loaded via server-side DataTables
            return View();
        }

        [HttpGet]
        public ActionResult AddMedicine()
        {
            var model = new CreateMedicineRequest();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMedicine(CreateMedicineRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _medicineService.CreateMedicine(model);

                if (response.Success)
                {
                    TempData["success"] = "Medicine saved successfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Message;

                    return View(model);
                }
            }
            TempData["error"] = "Failed to save Medicine.";

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditMedicine(int medicineId)
        {
            var response = await _medicineService.GetMedicine(medicineId);

            if (!response.Success) TempData["error"] = response.Message;

            var medicineToUpdate = new UpdateMedicineRequest
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                GenericName = response.Data.GenericName,
                Brand = response.Data.Brand,
                Manufacturer = response.Data.Manufacturer,
                DosageForm = response.Data.DosageForm,
                PackSize = response.Data.PackSize,
                QuantityPerUnit = response.Data.QuantityPerUnit
            };

            return View(medicineToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMedicine(UpdateMedicineRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _medicineService.UpdateMedicine(model);

            if (response.Success)
            {
                TempData["success"] = "Medicine updated successfully";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = response.Message;

                return View(model);
            }
        }

        public async Task<ActionResult> DeleteMedicine(int medicineId)
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var response = await _medicineService.DeleteMedicine(medicineId, token);

            if (response.Success)
            {
                TempData["success"] = "Medicine deleted successfully";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = response.Message;

                return RedirectToAction("Index");
            }
        }

        #region private methods
        private async Task LoadMedicineDropdown()
        {
            var supplierList = await _supplierService.GetAllAsync();

            ViewBag.suppliers = supplierList.Data.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }
        #endregion

        public async Task<JsonResult> GetAllMedicines()
        {
            var medicines = await _medicineService.GetMedicines();

            if (!medicines.Success)
            {
                return Json(new List<object>());
            }

            return Json(medicines.Data);
        }

        public async Task<JsonResult> GetMedicinesById(int id)
        {
            var medicine = await _medicineService.GetMedicines();
            if (!medicine.Success)
            {
                return Json(new List<object>());
            }

            var filtered = medicine.Data.Where(x => x.Id == id).ToList();

            return Json(filtered);
        }

        public async Task<JsonResult> GetMedicinesByCategory()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var medicines = await _selectionsService.GetMedicinesList(token);

            return Json(medicines);
        }

        public async Task<JsonResult> GetStockMedicinesByCategory()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var stock = await _selectionsService.GetStocks(token);

            return Json(stock);
        }

        public async Task<JsonResult> GetStockMedicineById(int id)
        {
            var stockmedicine = await _stockService.GetStock(id);

            if (!stockmedicine.Success)
            {
                return Json(new List<object>());
            }

            //var filtered = stockmedicine.Where(x => x.Id == id).ToList();
            return Json(stockmedicine.Data);
        }

        [HttpGet]
        public async Task<JsonResult> SearchMedicines(string query, int page)
        {
            var response = await _medicineService.SearchMedicines(query, page);

            if (response == null)
            {
                return Json(new List<MedicineResponse>());
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> GetMedicinesDataTable([FromBody] DataTableRequest request)
        {
            var response = await _medicineService.GetMedicinesForDataTable(request);
            return Json(response);
        }
    }
}
