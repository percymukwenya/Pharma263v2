using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentMade
{
    public class AddPaymentMadeDto
    {
        public int AccountPayableId { get; set; }
        public int SupplierId { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Amount Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountOwed { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Balance Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal BalanceOwed { get; set; }

        [Required]
        [Display(Name = "Amount to Pay")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount to Pay must be greater than 0")]
        public decimal AmountToPay { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}
