using System;

namespace Pharma263.MVC.DTOs.Returns
{
    public class ReturnsDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ReturnDestination { get; set; }
        public string ReturnReason { get; set; }
        public string ReturnStatus { get; set; }
        public string Medicine { get; set; }
        public string BatchNo { get; set; }
        public int SaleId { get; set; }
        public DateTime DateReturned { get; set; }
        public int SaleItemId { get; set; }
        public string MedicineName { get; set; }
        public int ReturnReasonId { get; set; }
        public int ReturnDestinationId { get; set; }
        public int ReturnStatusId { get; set; }
    }
}
