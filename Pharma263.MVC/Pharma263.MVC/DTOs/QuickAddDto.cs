using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Pharma263.MVC.DTOs
{
    public class QuickAddDto
    {
        [Required]
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        [Required]
        [DisplayName("Medicine Name")]
        public string MedicineName { get; set; }

        public string Code { get; set; }
        public string GenericName { get; set; }

        [Required]
        [DisplayName("Batch No")]
        public string BatchNo { get; set; }

        [Required]
        [DisplayName("Expiration Date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [DisplayName("Buying Price")]
        public double BuyingPrice { get; set; }

        [Required]
        [DisplayName("Selling Price")]
        public double SellingPrice { get; set; }

        [DisplayName("Quantity Per Unit")]
        public int QuantityPerUnit { get; set; }

        public string Notes { get; set; }
        [Required]
        public double Total { get; set; }

        [Required]
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [DisplayName("Total Quantity")]
        public int TotalQuantity { get; set; }
    }
}
