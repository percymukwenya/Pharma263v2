namespace Pharma263.MVC.DTOs.Returns
{
    public class SaleItemToReturnDto
    {
        public int SaleItemId { get; set; }
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public int ReturnReasonId { get; set; }
        public int ReturnDestinationId { get; set; }
    }
}
