using Pharma263.MVC.Models;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync(string token);
        Task<DashboardViewModel> GetDashboardWithTrendsAsync(string token, int days = 30);
    }
}
