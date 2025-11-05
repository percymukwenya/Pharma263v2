using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentReceived
{
    public class AddPaymentReceivedDto
    {
        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Amount Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountDue { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Balance Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal BalanceDue { get; set; }

        [Required]
        [Display(Name = "Amount to Receive")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount to Receive must be greater than 0")]
        public decimal AmountToReceive { get; set; }

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
