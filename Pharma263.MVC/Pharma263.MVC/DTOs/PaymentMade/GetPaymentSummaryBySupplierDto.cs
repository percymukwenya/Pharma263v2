using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentMade
{
    public class GetPaymentSummaryBySupplierDto
    {
        public int SupplierId { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Number of Payments")]
        public int PaymentCount { get; set; }

        [Display(Name = "Total Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountPaid { get; set; }

        [Display(Name = "First Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FirstPaymentDate { get; set; }

        [Display(Name = "Last Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastPaymentDate { get; set; }
    }
}
