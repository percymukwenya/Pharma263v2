using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IReturnReasonService
    {
        Task<T> GetAllAsync<T>();
    }
}
