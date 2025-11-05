using Pharma263.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Pharma263.Domain.Interfaces.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardResponseModel> GetDashboardData(int lowStockAmount);
        Task<DashboardResponseModel> GetDashboardDataByDateRange(DateTime startDate, DateTime endDate, int lowStockAmount);
    }
}
