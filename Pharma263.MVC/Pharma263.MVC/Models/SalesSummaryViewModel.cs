namespace Pharma263.MVC.Models
{
    public class SalesSummaryViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string TopSellingProduct { get; set; }
    }
}
