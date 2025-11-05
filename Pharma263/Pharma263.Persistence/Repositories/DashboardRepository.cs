using Dapper;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models;
using Pharma263.Persistence.Contexts;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Persistence.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;

        public DashboardRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseModel> GetDashboardData(int lowStockAmount)
        {
            using var conn = _context.CreateConnection();

            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_GetDashboardData]", new
            {
                lowStockAmount
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            {
                var dashboardCountResponse = resultSet.Read<DashboardCount>().FirstOrDefault();

                var lowStocksResponse = resultSet.Read<LowStockModel>().ToList();

                return new DashboardResponseModel
                {
                    DashboardCounts = new DashboardCount
                    {
                        TotalCustomers = dashboardCountResponse.TotalCustomers,
                        TotalMedicines = dashboardCountResponse.TotalMedicines,
                        TotalStockItems = dashboardCountResponse.TotalStockItems,
                        TotalStockQuantity = dashboardCountResponse.TotalStockQuantity,
                        TotalSuppliers = dashboardCountResponse.TotalSuppliers,
                        TotalPurchases = dashboardCountResponse.TotalPurchases,
                        TotalPurchaseAmount = dashboardCountResponse.TotalPurchaseAmount,
                        TotalSales = dashboardCountResponse.TotalSales,
                        TotalSalesAmount = dashboardCountResponse.TotalSalesAmount
                    },
                    LowStocks = lowStocksResponse
                };
            }
        }

        public async Task<DashboardResponseModel> GetDashboardDataByDateRange(DateTime startDate, DateTime endDate, int lowStockAmount)
        {
            using var conn = _context.CreateConnection();

            // Use the new stored procedure that accepts date range parameters
            using var resultSet = await conn.QueryMultipleAsync("[Pharma263].[up_GetDashboardDataByDateRange]", new
            {
                startDate,
                endDate,
                lowStockAmount
            }, commandType: CommandType.StoredProcedure, commandTimeout: 120);
            {
                var dashboardCountResponse = resultSet.Read<DashboardCount>().FirstOrDefault();

                var lowStocksResponse = resultSet.Read<LowStockModel>().ToList();

                return new DashboardResponseModel
                {
                    DashboardCounts = new DashboardCount
                    {
                        TotalCustomers = dashboardCountResponse.TotalCustomers,
                        TotalMedicines = dashboardCountResponse.TotalMedicines,
                        TotalStockItems = dashboardCountResponse.TotalStockItems,
                        TotalStockQuantity = dashboardCountResponse.TotalStockQuantity,
                        TotalSuppliers = dashboardCountResponse.TotalSuppliers,
                        TotalPurchases = dashboardCountResponse.TotalPurchases,
                        TotalPurchaseAmount = dashboardCountResponse.TotalPurchaseAmount,
                        TotalSales = dashboardCountResponse.TotalSales,
                        TotalSalesAmount = dashboardCountResponse.TotalSalesAmount
                    },
                    LowStocks = lowStocksResponse
                };
            }
        }
    }
}
