using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.AccountsPayable;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class AccountsPayableService : IAccountsPayableService
    {
        private readonly IApiService _apiService;

        public AccountsPayableService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<GetAccountsPayableListDto>>> GetAccountsPayable()
        {
            var response = await _apiService.GetApiResponseAsync<List<GetAccountsPayableListDto>>("/api/AccountsPayable/GetAccountsPayable");

            return response;
        }

        public Task<ApiResponse<GetAccountsPayableDetailsDto>> GetAccountPayable(int id)
        {
            var response = _apiService.GetApiResponseAsync<GetAccountsPayableDetailsDto>($"/api/AccountsPayable/GetAccountPayable/{id}");

            return response;
        }

        public async Task<ApiResponse<List<GetSupplierAccountSummaryDto>>> GetSupplierAccountsSummary()
        {
            var response = await _apiService.GetApiResponseAsync<List<GetSupplierAccountSummaryDto>>("/api/AccountsPayable/GetSupplierAccountsSummary");

            return response;
        }

        public async Task<ApiResponse<bool>> AddAccountsPayable(AddAccountsPayableDto model)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/AccountsPayable/AddAccountsPayable", model);
        }

        public Task<ApiResponse<bool>> UpdateAccountsPayable(UpdateAccountsPayableDto model)
        {
            return _apiService.PutApiResponseAsync<bool>("/api/AccountsPayable/UpdateAccountsPayable", model);
        }

        public Task<ApiResponse<bool>> DeleteAccountsPayable(int id)
        {
            return _apiService.DeleteApiResponseAsync($"/api/AccountsPayable/DeleteAccountsPayable/{id}");
        }

        public async Task<ApiResponse<List<GetAccountsPayableListDto>>> GetPastDueAccountsPayable()
        {
            return await _apiService.GetApiResponseAsync<List<GetAccountsPayableListDto>>("/api/AccountsPayable/GetPastDueAccountsPayable");
        }
    }
}
