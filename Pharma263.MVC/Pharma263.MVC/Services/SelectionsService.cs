using Pharma263.Integration.Api;
using Pharma263.MVC.DTOs.PaymentMethods;
using Pharma263.MVC.Services.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class SelectionsService : ISelectionsService
    {
        private readonly IPharmaApiService _pharmaApiService;

        public SelectionsService(IPharmaApiService pharmaApiService)
        {
            _pharmaApiService = pharmaApiService;
        }

        public async Task<List<ListItemDto>> GetCustomersList(string token)
        {
            var response = await _pharmaApiService.GetCustomersList(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetCustomerTypes(string token)
        {
            var response = await _pharmaApiService.GetCustomerTypes(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetMedicinesList(string token)
        {
            var response = await _pharmaApiService.GetMedicinesList(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetPaymentMethods(string token)
        {
            var response = await _pharmaApiService.GetPaymentMethods(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetPurchaseStatuses(string token)
        {
            var response = await _pharmaApiService.GetPurchaseStatuses(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetSaleStatuses(string token)
        {
            var response = await _pharmaApiService.GetSaleStatuses(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetReturnDestinations(string token)
        {
            var response = await _pharmaApiService.GetReturnDestinations(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetReturnReasons(string token)
        {
            var response = await _pharmaApiService.GetReturnReasons(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetStocks(string token)
        {
            var response = await _pharmaApiService.GetStocks(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }

        public async Task<List<ListItemDto>> GetSuppliersList(string token)
        {
            var response = await _pharmaApiService.GetSuppliersList(token);

            // Manual mapping - identical properties, no AutoMapper overhead needed
            return response?.Select(r => new ListItemDto { Id = r.Id, Name = r.Name }).ToList();
        }
    }
}
