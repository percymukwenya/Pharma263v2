using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.PaymentReceived
{
    public class GetPaymentSummaryByCustomerDto
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Number of Payments")]
        public int PaymentCount { get; set; }

        [Display(Name = "Total Amount Received")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountReceived { get; set; }

        [Display(Name = "First Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FirstPaymentDate { get; set; }

        [Display(Name = "Last Payment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastPaymentDate { get; set; }
    }
}
