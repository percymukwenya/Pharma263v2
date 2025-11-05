namespace Pharma263.Domain.Models
{
    public class PurchaseByProductReportModel
    {
        public string MedicineName { get; set; }
        public double TotalPurchaseCost { get; set; }
        public int TotalTransactions { get; set; }
    }
}
