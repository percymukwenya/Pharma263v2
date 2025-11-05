using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.AccountsPayable
{
    public class GetAccountsPayableListDto
    {
        public int Id { get; set; }

        [Display(Name = "Supplier")]
        public string Supplier { get; set; }

        [Display(Name = "Amount Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountOwed { get; set; }

        [Display(Name = "Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal AmountPaid { get; set; }

        [Display(Name = "Balance Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal BalanceOwed { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Status")]
        public string AccountsPayableStatus { get; set; }

        public int SupplierId { get; set; }
    }
}
