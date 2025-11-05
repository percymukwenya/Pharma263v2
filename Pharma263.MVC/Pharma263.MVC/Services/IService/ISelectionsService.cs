using Pharma263.MVC.DTOs.PaymentMethods;
using Pharma263.MVC.DTOs.Stock;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ISelectionsService
    {
        Task<List<ListItemDto>> GetCustomerTypes(string token);
        Task<List<ListItemDto>> GetCustomersList(string token);
        Task<List<ListItemDto>> GetSuppliersList(string token);
        Task<List<ListItemDto>> GetMedicinesList(string token);
        Task<List<ListItemDto>> GetStocks(string token);
        Task<List<ListItemDto>> GetPaymentMethods(string token);
        Task<List<ListItemDto>> GetPurchaseStatuses(string token);
        Task<List<ListItemDto>> GetSaleStatuses(string token);
        Task<List<ListItemDto>> GetReturnReasons(string token);

        Task<List<ListItemDto>> GetReturnDestinations(string token);
    }
}
