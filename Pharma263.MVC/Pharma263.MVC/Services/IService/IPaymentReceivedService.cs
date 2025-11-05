using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.PaymentReceived;

namespace Pharma263.MVC.Services.IService
{
    public interface IPaymentReceivedService
    {
        Task<ApiResponse<List<GetPaymentReceivedListDto>>> GetPaymentsReceived();
        Task<ApiResponse<List<GetPaymentReceivedListDto>>> GetPaymentsReceivedFromCustomer(int customerId);
        Task<ApiResponse<GetPaymentReceivedDetailsDto>> GetPaymentReceived(int id);
        Task<ApiResponse<bool>> AddPaymentReceived(AddPaymentReceivedRequest model);
        Task<ApiResponse<bool>> UpdatePaymentReceived(UpdatePaymentReceivedDto model);
        Task<ApiResponse<bool>> DeletePaymentReceived(int id);
        Task<ApiResponse<List<GetPaymentSummaryByCustomerDto>>> GetPaymentSummaryByCustomer(DateTime startDate, DateTime endDate);
    }
}
