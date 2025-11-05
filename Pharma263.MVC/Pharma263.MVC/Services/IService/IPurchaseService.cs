using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IPurchaseService
    {
        Task<ApiResponse<List<PurchasesResponse>>> GetAllPurchases();
        Task<ApiResponse<PurchaseDetailsResponse>> GetPurchase(int id);
        Task<ApiResponse<bool>> CreatePurchase(CreatePurchaseRequest dto);
        Task<ApiResponse<bool>> UpdatePurchase(UpdatePurchaseRequest dto);
        Task<ApiResponse<bool>> DeletePurchase(int id);
        Task<byte[]> GetPurchaseInvoice(int id);
    }
}
