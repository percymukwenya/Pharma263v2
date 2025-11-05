using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.AccountsReceivable
{
    public class GetCustomerAccountSummaryDto
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Total Amount Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountDue { get; set; }

        [Display(Name = "Total Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountPaid { get; set; }

        [Display(Name = "Total Balance Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalBalanceDue { get; set; }

        [Display(Name = "Number of Accounts")]
        public int NumberOfAccounts { get; set; }

        [Display(Name = "Earliest Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EarliestDueDate { get; set; }
    }
}
