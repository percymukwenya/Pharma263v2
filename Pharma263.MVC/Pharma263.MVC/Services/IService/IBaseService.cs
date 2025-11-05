using Pharma263.MVC.DTOs;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IBaseService
    {
        ApiResponse responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
