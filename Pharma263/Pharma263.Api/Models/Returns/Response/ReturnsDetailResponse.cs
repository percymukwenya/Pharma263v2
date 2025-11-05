using System;

namespace Pharma263.Api.Models.Returns.Response
{
    public class ReturnsDetailResponse
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int SaleItemId { get; set; }
        public int StockId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int ReturnReasonId { get; set; }
        public string ReturnReason { get; set; }
        public int ReturnDestinationId { get; set; }
        public string ReturnDestination { get; set; }
        public int ReturnStatusId { get; set; }
        public string ReturnStatus { get; set; }
        public string Notes { get; set; }
        public DateTime DateReturned { get; set; }
    }
}
