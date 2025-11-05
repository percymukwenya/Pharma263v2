using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Purchases
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string PurchaseStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Discount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double GrandTotal { get; set; }

        public string Supplier { get; set; }
    }
}
