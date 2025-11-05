using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.AccountsPayable
{
    public class GetSupplierAccountSummaryDto
    {
        public int SupplierId { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Total Amount Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountOwed { get; set; }

        [Display(Name = "Total Amount Paid")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalAmountPaid { get; set; }

        [Display(Name = "Total Balance Owed")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalBalanceOwed { get; set; }

        [Display(Name = "Number of Accounts")]
        public int NumberOfAccounts { get; set; }

        [Display(Name = "Earliest Due Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EarliestDueDate { get; set; }
    }
}
