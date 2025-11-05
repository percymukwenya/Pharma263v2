using System;

namespace Pharma263.MVC.DTOs.Stock
{
    public class StockSelectionDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BatchNo { get; set; }
    }
}
