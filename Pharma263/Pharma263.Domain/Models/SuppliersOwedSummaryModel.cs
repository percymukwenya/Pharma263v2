namespace Pharma263.Domain.Models
{
    public class SuppliersOwedSummaryModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public double AmountOwed { get; set; }
    }
}
