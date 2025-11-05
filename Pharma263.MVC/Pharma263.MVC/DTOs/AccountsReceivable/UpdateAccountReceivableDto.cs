using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.AccountsReceivable
{
    public class UpdateAccountReceivableDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount Due")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Due must be greater than 0")]
        public decimal AmountDue { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Amount Paid")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid cannot be negative")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Balance Due")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance Due cannot be negative")]
        public decimal BalanceDue { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int AccountsReceivableStatusId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
    }
}
