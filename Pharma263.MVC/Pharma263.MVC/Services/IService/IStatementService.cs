using System.Threading.Tasks;
using System;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Common;

namespace Pharma263.MVC.Services.IService
{
    public interface IStatementService
    {
        Task<ApiResponse<CustomerStatementResponse>> GetCustomerStatementAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null);
        Task<ApiResponse<SupplierStatementResponse>> GetSupplierStatementAsync(int supplierId, DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> GetCustomerStatementPdfAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> GetSupplierStatementPdfAsync(int supplierId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
