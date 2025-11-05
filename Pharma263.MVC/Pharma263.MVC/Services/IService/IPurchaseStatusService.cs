using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IPurchaseStatusService
    {
        Task<T> GetAllAsync<T>(string token);
    }
}
