using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Purchases;
using Pharma263.MVC.Services.IService;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IMapper _mapper;

        public PurchaseController(IPurchaseService purchaseService, IMapper mapper)
        {
            _purchaseService = purchaseService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var response = await _purchaseService.GetAllPurchases();

            return View(response.Data);
        }

        [HttpGet]
        public ActionResult Purchase()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Purchase([FromBody] CreatePurchaseRequest data)
        {
            if (ModelState.IsValid)
            {
                var response = await _purchaseService.CreatePurchase(data);

                if (response.Success == true)
                {
                    TempData["success"] = "Purchase saved successfully";
                    return Json(new { error = false, message = "Purchase saved successfully" });
                }
            }

            TempData["error"] = "Failed to save Purchase.";

            return Json(new { error = true, message = "failed to save Purchase" });
        }

        [HttpGet]
        public async Task<ActionResult> EditPurchase(int purchaseId)
        {
            var purchase = await _purchaseService.GetPurchase(purchaseId);

            if (purchase != null)
            {
                var purchaseToUpdate = _mapper.Map<PurchaseDetailsDto>(purchase.Data);

                return View(model: purchaseId);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePurchase([FromBody] UpdatePurchaseRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _purchaseService.UpdatePurchase(model);

                if (response.Success == true)
                {
                    TempData["success"] = "Purchase updated Successfully";

                    return Json(new { error = false, message = "Purchase updated successfully" });

                    //return RedirectToAction("Index");
                }
            }
            TempData["error"] = "Failed to Update Purchase.";
            return Json(new { error = true, message = "failed to Update Purchase" });
        }

        //[HttpDelete]

        public async Task<ActionResult> DeletePurchase(int purchaseId)
        {
            if (purchaseId > 0)
            {
                var result = await _purchaseService.DeletePurchase(purchaseId);

                if (!result.Success)
                {
                    TempData["error"] = "Failed to delete Purchase.";
                    return Json(new { error = true, message = "failed to delete Purchase" });
                }

                TempData["success"] = "Purchase deleted successfully";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Failed to delete Purchase.";
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<ActionResult> PurchaseInvoice(int purchaseId)
        {
            if (purchaseId > 0)
            {
                var bytes = await _purchaseService.GetPurchaseInvoice(purchaseId);

                return File(bytes, "application/pdf");
            }
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetPurchase(int purchaseId)
        {
            var model = await _purchaseService.GetPurchase(purchaseId);

            return Json(model.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var response = await _purchaseService.GetPurchase(id);
            
            if (!response.Success)
            {
                TempData["error"] = "Purchase not found";
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }

    }
}
