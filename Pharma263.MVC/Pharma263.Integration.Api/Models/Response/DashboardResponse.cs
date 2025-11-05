using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Response
{
    public class DashboardResponse
    {
        public DashboardCount DashboardCounts { get; set; }
        public List<LowStockModel> LowStocks { get; set; }
    }
}
