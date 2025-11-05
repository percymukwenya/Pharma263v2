using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.AccountsPayable
{
    public class AddAccountsPayableDto
    {
        [Required]
        [Display(Name = "Amount Owed")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Owed must be greater than 0")]
        public decimal AmountOwed { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Amount Paid")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid cannot be negative")]
        public decimal AmountPaid { get; set; } = 0;

        [Required]
        [Display(Name = "Balance Owed")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance Owed cannot be negative")]
        public decimal BalanceOwed { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int AccountsPayableStatusId { get; set; } = 1; // Default to Outstanding

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }
    }
}
