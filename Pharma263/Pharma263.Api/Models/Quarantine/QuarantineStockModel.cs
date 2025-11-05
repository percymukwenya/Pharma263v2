using System;

namespace Pharma263.Api.Models.Quarantine
{
    public class QuarantineStockModel
    {
        public int Id { get; set; }
        public int TotalQuantity { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNumber { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public  int ReturnReasonId { get; set; }
    }
}
