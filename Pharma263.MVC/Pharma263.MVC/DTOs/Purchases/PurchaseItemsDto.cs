using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Purchases
{
    public class PurchaseItemsDto
    {
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Price { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Amount { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double BuyingPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double SellingPrice { get; set; }
    }
}