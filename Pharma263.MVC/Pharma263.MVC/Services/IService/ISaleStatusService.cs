using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ISaleStatusService
    {
        Task<T> GetAllAsync<T>();
    }
}
