namespace Pharma263.MVC.DTOs.Returns
{
    public class ReturnItemDto
    {
        public int SaleItemId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int QuantityToReturn { get; set; }

        public int ReturnReasonId { get; set; }
        public int ReturnDestinationId { get; set; }
    }
}
