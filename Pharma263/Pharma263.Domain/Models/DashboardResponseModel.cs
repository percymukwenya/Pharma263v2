using System.Collections.Generic;

namespace Pharma263.Domain.Models
{
    public class DashboardResponseModel
    {
        public DashboardCount DashboardCounts { get; set; }
        public List<LowStockModel> LowStocks { get; set; }
    }
}
