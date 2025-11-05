using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.AccountsPayable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IAccountsPayableService
    {
        Task<ApiResponse<List<GetAccountsPayableListDto>>> GetAccountsPayable();
        Task<ApiResponse<GetAccountsPayableDetailsDto>> GetAccountPayable(int id);
        Task<ApiResponse<bool>> AddAccountsPayable(AddAccountsPayableDto model);
        Task<ApiResponse<bool>> UpdateAccountsPayable(UpdateAccountsPayableDto model);
        Task<ApiResponse<bool>> DeleteAccountsPayable(int id);
        Task<ApiResponse<List<GetAccountsPayableListDto>>> GetPastDueAccountsPayable();
        Task<ApiResponse<List<GetSupplierAccountSummaryDto>>> GetSupplierAccountsSummary();
    }
}
