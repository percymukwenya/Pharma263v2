using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IReturnDestinationService
    {
        Task<T> GetAllAsync<T>();
    }
}
