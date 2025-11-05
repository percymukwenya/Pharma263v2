using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.AccountsReceivable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IAccountsReceivableService
    {
        Task<ApiResponse<List<GetAccountsReceivableListDto>>> GetAccountsReceivable();
        Task<ApiResponse<GetAccountsReceivableDetailsDto>> GetAccountReceivable(int id);
        Task<ApiResponse<bool>> AddAccountsReceivable(AddAccountReceivableDto model);
        Task<ApiResponse<bool>> UpdateAccountsReceivable(UpdateAccountReceivableDto model);
        Task<ApiResponse<bool>> DeleteAccountsReceivable(int id);
        Task<ApiResponse<List<GetAccountsReceivableListDto>>> GetPastDueAccountsReceivable();
        Task<ApiResponse<List<GetCustomerAccountSummaryDto>>> GetCustomerAccountsSummary();
        Task<ApiResponse<List<GetAccountsReceivableAgingDto>>> GetAccountsReceivableAging();
    }
}
