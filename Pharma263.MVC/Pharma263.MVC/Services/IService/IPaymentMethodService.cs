using Pharma263.Integration.Api.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IPaymentMethodService
    {
        Task<List<SelectListResponse>> GetAllAsync();
    }
}
