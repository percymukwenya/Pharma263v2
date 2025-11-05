using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.AccountsReceivable.Request
{
    public class UpdateAccountReceivableModel
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid accounts receivable ID")]
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount due must be greater than or equal to 0")]
        public decimal AmountDue { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount paid must be greater than or equal to 0")]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance due must be greater than or equal to 0")]
        public decimal BalanceDue { get; set; }

        [Required(ErrorMessage = "Accounts receivable status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid accounts receivable status")]
        public int AccountsReceivableStatusId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Sale is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale")]
        public int SaleId { get; set; }
    }
}
