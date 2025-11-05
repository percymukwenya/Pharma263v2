using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Stock;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddStock()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStock([FromBody] StockItemsViewModel stocks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _stockService.AddStock([.. stocks.StockItems.Select(x => new AddStockRequest
                {
                    MedicineName = x.MedicineName,
                    BatchNo = x.BatchNo,
                    ExpiryDate = x.ExpiryDate,
                    BuyingPrice = x.BuyingPrice,
                    SellingPrice = x.SellingPrice,
                    TotalQuantity = x.TotalQuantity
                })]);

                if (response.Success == true)
                {
                    TempData["success"] = "Stock saved successfully";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = response.Message;

                    return View();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, message = "An error occurred while processing the batch stock addition" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditStock(int stockId)
        {
            var response = await _stockService.GetStock(stockId);

            if (response.Success == true)
            {
                var stockToUpdate = new UpdateStockRequest
                {
                    Id = response.Data.Id,
                    MedicineId = response.Data.MedicineId,
                    MedicineName = response.Data.MedicineName,
                    ExpiryDate = response.Data.ExpiryDate,
                    BatchNo = response.Data.BatchNo,
                    BuyingPrice = response.Data.BuyingPrice,
                    SellingPrice = response.Data.SellingPrice,
                    TotalQuantity = response.Data.TotalQuantity
                };

                return View(stockToUpdate);
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStock(UpdateStockRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _stockService.UpdateStock(model);

            if (response.Success == true)
            {
                TempData["success"] = "Stock saved successfully";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = response.Message;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStockJson([FromBody] UpdateStockRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            try
            {
                var response = await _stockService.UpdateStock(model);

                if (response.Success)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, message = "An error occurred while updating the stock" });
            }
        }

        [HttpGet]
        public IActionResult ImportStock()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportStockFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "No file uploaded" });
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { success = false, message = "Invalid file format. Please upload an Excel file (.xlsx)" });
            }

            try
            {
                var response = await _stockService.ImportStockFromExcel(file);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    TempData["error"] = response.Message;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, message = "An error occurred while processing the Excel file" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetStocksDataTable([FromBody] DataTableRequest request)
        {
            var response = await _stockService.GetStocksForDataTable(request);
            return Json(response);
        }
    }

    public class StockItemsViewModel
    {
        public List<StockItem> StockItems { get; set; }
    }

    public class StockItem
    {
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
