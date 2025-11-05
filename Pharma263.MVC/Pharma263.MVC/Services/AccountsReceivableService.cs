using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.AccountsReceivable;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class AccountsReceivableService : IAccountsReceivableService
    {
        private readonly IApiService _apiService;

        public AccountsReceivableService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<GetAccountsReceivableListDto>>> GetAccountsReceivable()
        {
            var response = await _apiService.GetApiResponseAsync<List<GetAccountsReceivableListDto>>("/api/AccountsReceivable/GetAccountsReceivable");

            return response;
        }

        public async Task<ApiResponse<GetAccountsReceivableDetailsDto>> GetAccountReceivable(int id)
        {
            return await _apiService.GetApiResponseAsync<GetAccountsReceivableDetailsDto>($"/api/AccountsReceivable/GetAccountReceivable/{id}");
        }

        public async Task<ApiResponse<List<GetAccountsReceivableAgingDto>>> GetAccountsReceivableAging()
        {
            return await _apiService.GetApiResponseAsync<List<GetAccountsReceivableAgingDto>>("/api/AccountsReceivable/GetAccountsReceivableAging");
        }

        public async Task<ApiResponse<List<GetCustomerAccountSummaryDto>>> GetCustomerAccountsSummary()
        {
            return await _apiService.GetApiResponseAsync<List<GetCustomerAccountSummaryDto>>("/api/AccountsReceivable/GetCustomerAccountsSummary");
        }

        public async Task<ApiResponse<List<GetAccountsReceivableListDto>>> GetPastDueAccountsReceivable()
        {
            return await _apiService.GetApiResponseAsync<List<GetAccountsReceivableListDto>>("/api/AccountsReceivable/GetPastDueAccountsReceivable");
        }

        public async Task<ApiResponse<bool>> AddAccountsReceivable(AddAccountReceivableDto model)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/AccountsReceivable/AddAccountsReceivable", model);
        }

        public async Task<ApiResponse<bool>> UpdateAccountsReceivable(UpdateAccountReceivableDto model)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/AccountsReceivable/UpdateAccountsReceivable", model);
        }

        public async Task<ApiResponse<bool>> DeleteAccountsReceivable(int id)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/AccountsReceivable/DeleteAccountsReceivable/{id}");
        }
    }
}
