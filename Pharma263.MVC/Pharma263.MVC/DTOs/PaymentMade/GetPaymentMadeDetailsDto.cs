using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentMade
{
    public class GetPaymentMadeDetailsDto
    {
        public int Id { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        public int AccountPayableId { get; set; }
        public int SupplierId { get; set; }
    }
}
