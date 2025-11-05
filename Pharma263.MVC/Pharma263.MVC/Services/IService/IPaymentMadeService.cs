using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.PaymentMade;

namespace Pharma263.MVC.Services.IService
{
    public interface IPaymentMadeService
    {
        Task<ApiResponse<List<GetPaymentMadeListDto>>> GetPaymentsMade();
        Task<ApiResponse<List<GetPaymentMadeListDto>>> GetPaymentsMadeToSupplier(int supplierId);
        Task<ApiResponse<GetPaymentMadeDetailsDto>> GetPaymentMade(int id);
        Task<ApiResponse<bool>> AddPaymentMade(AddPaymentMadeRequest model);
        Task<ApiResponse<bool>> UpdatePaymentMade(UpdatePaymentMadeDto model);
        Task<ApiResponse<bool>> DeletePaymentMade(int id);
        Task<ApiResponse<List<GetPaymentSummaryBySupplierDto>>> GetPaymentSummaryBySupplier(DateTime startDate, DateTime endDate);
    }
}
