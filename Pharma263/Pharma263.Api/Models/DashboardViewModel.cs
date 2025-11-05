using System.Collections.Generic;

namespace Pharma263.Api.Models
{
    public class DashboardViewModel
    {
        public int Suppliers { get; set; }
        public int Purchases { get; set; }
        public int TotalPurchases { get; set; }
        public int Medicines { get; set; }
        public int Stocks { get; set; }
        public int TotalSales { get; set; }
        public int Sales { get; set; }

        public IEnumerable<StockModel> LowStocks { get; set; }

        public DashboardViewModel()
        {
            LowStocks = new List<StockModel>();
        }
    }
}
