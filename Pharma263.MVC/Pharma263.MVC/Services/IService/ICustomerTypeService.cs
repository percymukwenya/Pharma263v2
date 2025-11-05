using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ICustomerTypeService
    {
        Task<T> GetAllAsync<T>(string token);
    }
}
