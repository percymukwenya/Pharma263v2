using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Sales;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class SaleController : BaseController
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        public ActionResult Index()
        {
            // Return empty view - data will be loaded via server-side DataTables
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetSalesDataTable([FromBody] DataTableRequest request)
        {
            var response = await _saleService.GetSalesForDataTable(request);
            return Json(response);
        }

        [HttpGet]
        public ActionResult AddSale()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSale([FromBody] AddSaleDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _saleService.CreateSale(data);

                if (response.Success)
                {
                    TempData["success"] = "Sale saved successfully";

                    return Json(new { error = false, message = "Sale saved successfully" });
                }
                else
                {
                    TempData["error"] = "Failed to save Sale.";
                }
            }

            TempData["error"] = "Failed to save Sale.";

            return Json(new { error = true, message = "failed to save sale" });
        }

        public async Task<ActionResult> DeleteSale(int saleId)
        {
            if (saleId > 0)
            {
                var response = await _saleService.DeleteSale(saleId);

                if (response.Success == true)
                {
                    TempData["success"] = "Sale deleted successfully";

                    return RedirectToAction("Index");
                }

                TempData["error"] = "Failed to delete sale.";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> SaleInvoice(int saleId)
        {
            if (saleId > 0)
            {
                var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

                var bytes = await _saleService.GetSaleInvoice(saleId);

                return File(bytes, "application/pdf");
            }
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetSale(int saleId)
        {
            var model = await _saleService.GetSale(saleId);

            return Json(model.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var response = await _saleService.GetSale(id);
            
            if (!response.Success)
            {
                TempData["error"] = "Sale not found";
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }
    }
}
