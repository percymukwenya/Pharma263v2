using Pharma263.MVC.DTOs.Customer;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IQuarantineService
    {
        Task<T> GetAllAsync<T>(string token);
    }
}
