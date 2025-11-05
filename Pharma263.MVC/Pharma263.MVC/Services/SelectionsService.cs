using AutoMapper;
using Pharma263.Integration.Api;
using Pharma263.MVC.DTOs.PaymentMethods;
using Pharma263.MVC.Services.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class SelectionsService : ISelectionsService
    {
        private readonly IPharmaApiService _pharmaApiService;
        private readonly IMapper _mapper;

        public SelectionsService(IPharmaApiService pharmaApiService, IMapper mapper)
        {
            _pharmaApiService = pharmaApiService;
            _mapper = mapper;
        }

        public async Task<List<ListItemDto>> GetCustomersList(string token)
        {
            var response = await _pharmaApiService.GetCustomersList(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetCustomerTypes(string token)
        {
            var response = await _pharmaApiService.GetCustomerTypes(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetMedicinesList(string token)
        {
            var response = await _pharmaApiService.GetMedicinesList(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetPaymentMethods(string token)
        {
            var response = await _pharmaApiService.GetPaymentMethods(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetPurchaseStatuses(string token)
        {
            var response = await _pharmaApiService.GetPurchaseStatuses(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetSaleStatuses(string token)
        {
            var response = await _pharmaApiService.GetSaleStatuses(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetReturnDestinations(string token)
        {
            var response = await _pharmaApiService.GetReturnDestinations(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetReturnReasons(string token)
        {
            var response = await _pharmaApiService.GetReturnReasons(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetStocks(string token)
        {
            var response = await _pharmaApiService.GetStocks(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }

        public async Task<List<ListItemDto>> GetSuppliersList(string token)
        {
            var response = await _pharmaApiService.GetSuppliersList(token);

            if (response != null)
            {
                var details = _mapper.Map<List<ListItemDto>>(response);

                return details;
            }

            return null;
        }
    }
}
