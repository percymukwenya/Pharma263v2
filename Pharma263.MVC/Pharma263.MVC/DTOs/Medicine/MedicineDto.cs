using System;

namespace Pharma263.MVC.DTOs.Medicine
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string DosageForm { get; set; } = string.Empty;
        public string PackSize { get; set; } = string.Empty;
        public string BatchNo { get; set; } = string.Empty;
        public DateTimeOffset ExpiryDate { get; set; }
        public double BuyingPrice { get; set; }
        public double SellingPrice { get; set; }
        public int QuantityPerUnit { get; set; }
        public string Supplier { get; set; }
    }
}
