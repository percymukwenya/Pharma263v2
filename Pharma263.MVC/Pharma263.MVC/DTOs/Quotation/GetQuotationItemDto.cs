using System;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class GetQuotationItemDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public int StockId { get; set; }
        public int MedicineId { get; set; }
        public string BatchNo { get; set; }
        public string MedicineName { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public int QuotationId { get; set; }
        public decimal Discount { get; set; }
    }
}
