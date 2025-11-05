namespace Pharma263.MVC.Models
{
    public class ReturnItemViewModel
    {
        public int SaleItemId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int QuantityToReturn { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public int SelectedReturnReasonId { get; set; }
        public int SelectedReturnDestinationId { get; set; }
        public bool IsReturned { get; set; }
    }
}