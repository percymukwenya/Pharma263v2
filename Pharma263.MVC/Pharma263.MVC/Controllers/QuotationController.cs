using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Quotation;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class QuotationController : Controller
    {
        private readonly IQuotationService _quotationService;
        private readonly ISaleService _saleService;

        public QuotationController(IQuotationService quotationService, ISaleService saleService)
        {
            _quotationService = quotationService;
            _saleService = saleService;
        }

        public async Task<ActionResult> Index()
        {
            var response = await _quotationService.GetQuotations();

            return View(response.Data);
        }

        [HttpGet]
        public ActionResult AddQuotation()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddQuotation([FromBody] AddQuotationDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _quotationService.CreateQuotation(data);

                if (response.Success)
                {
                    TempData["success"] = "Quotation saved successfully";

                    return Json(new { error = false, message = "Quotation saved successfully" });
                }
                else
                {
                    TempData["error"] = "Failed to save Quotation.";

                    return Json(new { error = true, message = "Failed to save Quotation" });
                }
            }

            TempData["error"] = "Failed to save Quotation.";

            return Json(new { error = true, message = "Failed to save Quotation" });
        }

        [HttpGet]
        public async Task<ActionResult> EditQuotation(int quotationId)
        {
            var quotation = await _quotationService.GetQuotation(quotationId);

            if (quotation != null && quotation.Success)
            {
                var saleToUpdate = quotation.Data;

                return View(model: quotationId);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> EditQuotation([FromBody] UpdateQuotationDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _quotationService.UpdateQuotation(model);

                if (response.Success == true)
                {
                    TempData["success"] = "Quotation updated successfully";
                    return Json(new { error = false, message = "Quotation updated successfully" });
                }
                else
                {
                    TempData["error"] = response.Message;
                    return Json(new { error = true, message = "Failed to Update Quotation" });
                }
            }
            TempData["error"] = "Failed to Update Quotation.";
            return Json(new { error = true, message = "failed to Update Quotation" });
        }

        public async Task<ActionResult> DeleteQuotation(int quotationId)
        {
            if (quotationId > 0)
            {
                var result = await _quotationService.DeleteQuotation(quotationId);

                if (result.Success)
                {
                    TempData["success"] = "Quotation deleted successfully";
                }
                else
                {
                    TempData["error"] = result.Message;
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> QuotationDoc(int quotationId)
        {
            if (quotationId > 0)
            {
                var bytes = await _quotationService.GetQoutationDoc(quotationId);

                return File(bytes, "application/pdf");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ConvertToSale(int id)
        {
            var quotationResponse = await _quotationService.GetQuotation(id);

            if (quotationResponse == null && quotationResponse.Success)
            {
                var quotation = quotationResponse.Data;
                if (quotation == null)
                {
                    TempData["error"] = "Quotation not found.";
                    return RedirectToAction("Index");
                }

                // Map the quotation to a sale request
                var saleRequest = new CreateSaleRequest
                {
                    CustomerId = quotation.CustomerId,
                    Discount = (decimal)quotation.Discount,
                    SalesDate = DateTime.Now,
                    GrandTotal = (decimal)quotation.GrandTotal,
                    PaymentMethodId = 1,
                    Notes = quotation.Notes,
                    SaleStatusId = 1,
                    QuotationId = id,
                    Total = (decimal)quotation.Total,
                    Items = [.. quotation.Items.Select(x => new SaleItemModel
                {
                    Amount = x.Amount,
                    Discount = x.Discount,
                    MedicineName = x.MedicineName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    StockId = x.StockId
                })]
                };

                // Call the API to convert quotation to sale
                var response = await _saleService.CreateSaleFromQuotation(saleRequest);

                if (response != null && response.Success)
                {
                    TempData["success"] = "Quotation successfully converted to sale.";
                    return RedirectToAction("Details", "Sale", new { id = response.Data });
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Failed to convert quotation to sale.";
                    return RedirectToAction("Details", new { id });
                }
            }

            TempData["error"] = "Failed to convert quotation to sale.";
            return RedirectToAction("Index");

        }

        public async Task<JsonResult> GetQuotation(int quotationId)
        {
            var model = await _quotationService.GetQuotation(quotationId);

            return Json(model.Data);
        }
    }
}
