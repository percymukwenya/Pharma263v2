using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharma263.MVC.DTOs.PaymentMethods;
using Pharma263.MVC.DTOs.Returns;
using Pharma263.MVC.Models;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class ReturnController : Controller
    {
        private readonly IReturnService _returnService;
        private readonly IReturnReasonService _returnReasonService;
        private readonly IReturnDestinationService _returnDestinationService;
        private readonly ISaleService _saleService;

        public ReturnController(IReturnService returnService, ISaleService saleService,
            IReturnReasonService returnReasonService, IReturnDestinationService returnDestinationService)
        {
            _returnService = returnService;
            _saleService = saleService;
            _returnReasonService = returnReasonService;
            _returnDestinationService = returnDestinationService;
        }

        public async Task<ActionResult> Index()
        {
            var response = await _returnService.GetAllAsync();

            return View(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Process(int saleId)
        {
            var response = await _saleService.GetSale(saleId);

            if (response.Success)
            {
                var sale = response.Data;

                if (sale == null)
                {
                    TempData["error"] = "Sale not found.";

                    return RedirectToAction("Index", "Sale");
                }

                var returnItems = new List<ReturnItemViewModel>();

                foreach (var item in sale.Items)
                {
                    // Check if the item already has a return associated with it
                    var isReturned = false; // We need to implement logic to check if the item is already returned

                    returnItems.Add(new ReturnItemViewModel
                    {
                        SaleItemId = item.Id,
                        MedicineName = item.MedicineName,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        Total = item.Amount,
                        IsReturned = isReturned
                    });
                }

                var returnReasons = await GetReturnReasonsFromApi();
                var returnDestinations = await GetReturnDestinationsFromApi();

                var model = new ProcessReturnViewModel
                {
                    SaleId = saleId,
                    CustomerName = sale.CustomerName,
                    SaleDate = sale.SalesDate.ToString(),
                    SaleTotal = (decimal)sale.Total,
                    SaleStatus = sale.SaleStatus,
                    ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text"),
                    ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text"),
                    ReturnItems = returnItems
                };

                return View(model);
            }
            else
            {
                TempData["error"] = "Failed to process return.";

                return View();
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Process(ProcessReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload the select lists
                var returnReasons = await GetReturnReasonsFromApi();
                var returnDestinations = await GetReturnDestinationsFromApi();
                model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                return View(model);
            }

            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var saleItemsToReturn = new List<SaleItemToReturnDto>();

            // Only include items with quantity to return > 0
            foreach (var item in model.ReturnItems.Where(i => i.QuantityToReturn > 0))
            {
                // Validate that the return quantity is not greater than the original quantity
                if (item.QuantityToReturn > item.Quantity)
                {
                    ModelState.AddModelError("", $"Return quantity for {item.MedicineName} cannot exceed original quantity.");
                    var returnReasons = await GetReturnReasonsFromApi();
                    var returnDestinations = await GetReturnDestinationsFromApi();
                    model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                    model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                    return View(model);
                }

                // Validate that a return reason and destination are selected
                if (item.SelectedReturnReasonId == 0)
                {
                    ModelState.AddModelError("", $"Please select a return reason for {item.MedicineName}.");
                    var returnReasons = await GetReturnReasonsFromApi();
                    var returnDestinations = await GetReturnDestinationsFromApi();
                    model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                    model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                    return View(model);
                }

                if (item.SelectedReturnDestinationId == 0)
                {
                    ModelState.AddModelError("", $"Please select a return destination for {item.MedicineName}.");
                    var returnReasons = await GetReturnReasonsFromApi();
                    var returnDestinations = await GetReturnDestinationsFromApi();
                    model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                    model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                    return View(model);
                }

                saleItemsToReturn.Add(new SaleItemToReturnDto
                {
                    SaleItemId = item.SaleItemId,
                    Quantity = item.QuantityToReturn,
                    ReturnReasonId = item.SelectedReturnReasonId,
                    ReturnDestinationId = item.SelectedReturnDestinationId
                });
            }

            if (!saleItemsToReturn.Any())
            {
                ModelState.AddModelError("", "No items selected for return.");
                var returnReasons = await GetReturnReasonsFromApi();
                var returnDestinations = await GetReturnDestinationsFromApi();
                model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                return View(model);
            }

            var request = new ProcessReturnRequestDto
            {
                SaleId = model.SaleId,
                SaleItemsToReturn = saleItemsToReturn
            };

            var response = await _returnService.ProcessReturn(request);

            if (response != null && response.Success)
            {
                TempData["success"] = "Return processed successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Failed to process return.";
                var returnReasons = await GetReturnReasonsFromApi();
                var returnDestinations = await GetReturnDestinationsFromApi();
                model.ReturnReasonsSelectList = new SelectList(returnReasons, "Value", "Text");
                model.ReturnDestinationsSelectList = new SelectList(returnDestinations, "Value", "Text");
                return View(model);
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetReturnReasonsFromApi()
        {
            var returnReasons = await _returnReasonService.GetAllAsync<List<ListItemDto>>();
            return returnReasons.Select(reason => new SelectListItem
            {
                Value = reason.Id.ToString(),
                Text = reason.Name
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetReturnDestinationsFromApi()
        {
            var returnDestinations = await _returnDestinationService.GetAllAsync<List<ListItemDto>>();
            return returnDestinations.Select(reason => new SelectListItem
            {
                Value = reason.Id.ToString(),
                Text = reason.Name
            });
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var response = await _returnService.GetAsync(id);

            if (response == null || !response.Success || response.Data == null)
            {
                TempData["error"] = "Return not found.";
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }
    }
}
