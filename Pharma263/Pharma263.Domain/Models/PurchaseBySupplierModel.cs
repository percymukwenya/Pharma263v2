using System;

namespace Pharma263.Domain.Models
{
    public class PurchaseBySupplierModel
    {
        public string MedicineName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
