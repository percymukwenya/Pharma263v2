using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.DTOs;
using Pharma263.MVC.DTOs.StoreSettings;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class StoreSettingsController : BaseController
    {
        private readonly IStoreSettingService _storeSettingService;

        public StoreSettingsController(IStoreSettingService storeSettingService)
        {
            _storeSettingService = storeSettingService;
        }

        [HttpGet]
        public async Task<ActionResult> StoreSetting()
        {
            var token = HttpContext.Session.GetString(StaticDetails.SessionToken);

            var settings = await _storeSettingService.GetAsync();

            if (settings != null)
            {
                // Ensure logo has a value - use default if empty
                if (string.IsNullOrEmpty(settings.Logo))
                {
                    settings.Logo = "/images/pharma263Logo.jpg";
                }
                return View(settings);
            }
            
            // Return new model with default logo
            var defaultSettings = new StoreSettingsDetailsDto
            {
                Logo = "/images/pharma263Logo.jpg"
            };
            return View(defaultSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StoreSetting(UpdateStoreSettingsDto model, IFormFile logoPostedFileBase)
        {
            if (!ModelState.IsValid) 
                return View(model);

            var settings = await _storeSettingService.GetAsync();
            string logoPath = "/images/pharma263Logo.jpg"; // Always use default logo for now

            if (logoPostedFileBase != null && logoPostedFileBase.Length > 0)
            {
                // Create unique filename
                string fileName = Path.GetFileName(logoPostedFileBase.FileName);
                string fileExtension = Path.GetExtension(fileName);
                string uniqueFileName = $"logo_{Guid.NewGuid().ToString("N")[..8]}{fileExtension}";
                
                // Save to wwwroot/images directory
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder); // Ensure directory exists
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await logoPostedFileBase.CopyToAsync(fileStream);
                }
                
                logoPath = $"/images/{uniqueFileName}";
            }

            settings.Id = settings.Id;
            settings.StoreName = model.StoreName;
            settings.Phone = model.Phone;
            settings.Currency = model.Currency;
            settings.Email = model.Email;
            settings.Address = model.Address;
            settings.MCAZLicence = model.MCAZLicence;
            settings.VATNumber = model.VATNumber;
            settings.BankingDetails = model.BankingDetails;
            settings.ReturnsPolicy = model.ReturnsPolicy;
            settings.Logo = logoPath;

            var settingToUpdate = new UpdateStoreSettingsDto
            {
                Id = settings.Id,
                Logo = logoPath,
                StoreName = settings.StoreName,
                Phone = settings.Phone,
                Currency = settings.Currency,
                Email = settings.Email,
                Address = settings.Address,
                MCAZLicence = settings.MCAZLicence,
                VATNumber = settings.VATNumber,
                BankingDetails = settings.BankingDetails,
                ReturnsPolicy = settings.ReturnsPolicy
            };


            var result = await _storeSettingService.UpdateAsync(settingToUpdate);

            if (result.Success)
            {
                TempData["success"] = "Store setting updated successfully";
                return RedirectToAction("StoreSetting");
            }
            else
            {
                TempData["error"] = "Could not save settings";

                return RedirectToAction("StoreSetting");
            }
        }
    }
}
