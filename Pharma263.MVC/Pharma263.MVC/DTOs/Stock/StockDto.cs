using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Stock
{
    public class StockDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }

        public string BatchNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double BuyingPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double SellingPrice { get; set; }

        public int TotalQuantity { get; set; }
    }
}
