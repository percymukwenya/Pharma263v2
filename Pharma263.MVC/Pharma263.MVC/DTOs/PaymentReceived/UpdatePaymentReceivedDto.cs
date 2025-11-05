using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentReceived
{
    public class UpdatePaymentReceivedDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount Received")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Received must be greater than 0")]
        public decimal AmountReceived { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }

        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}
