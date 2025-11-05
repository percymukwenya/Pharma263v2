using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.AccountsReceivable
{
    public class GetAccountsReceivableListDto
    {
        public int Id { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }

        [Display(Name = "Amount Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountDue { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Balance Due")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal BalanceDue { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Status")]
        public string AccountsReceivableStatus { get; set; }

        public int CustomerId { get; set; }
    }
}
