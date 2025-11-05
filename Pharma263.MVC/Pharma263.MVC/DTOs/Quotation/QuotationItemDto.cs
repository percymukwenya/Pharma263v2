using System;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class QuotationItemDto
    {
        public int StockId { get; set; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
    }
}