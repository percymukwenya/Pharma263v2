using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentReceived
{
    public class GetPaymentReceivedListDto
    {
        public int Id { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }

        [Display(Name = "Amount Received")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountReceived { get; set; }

        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }
    }
}
