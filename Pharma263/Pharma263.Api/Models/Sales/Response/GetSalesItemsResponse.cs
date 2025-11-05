using System;

namespace Pharma263.Api.Models.Sales.Response
{
    public class GetSalesItemsResponse
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
        public int SalesId { get; set; }
    }
}
