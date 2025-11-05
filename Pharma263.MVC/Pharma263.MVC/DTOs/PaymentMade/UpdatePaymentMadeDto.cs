using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentMade
{
    public class UpdatePaymentMadeDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Paid must be greater than 0")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }

        public int AccountPayableId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}
