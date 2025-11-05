using System;

namespace Pharma263.Integration.Api.Models.Response
{
    public class StockSelectionResponse
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BatchNo { get; set; }
    }
}
