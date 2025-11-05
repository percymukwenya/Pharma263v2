using System;

namespace Pharma263.MVC.Models
{
    public class PurchaseBySupplierViewModel
    {
        public string MedicineName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
