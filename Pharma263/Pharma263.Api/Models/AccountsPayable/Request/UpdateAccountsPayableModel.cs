using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.AccountsPayable.Request
{
    public class UpdateAccountsPayableModel
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid accounts payable ID")]
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount owed must be greater than or equal to 0")]
        public decimal AmountOwed { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount paid must be greater than or equal to 0")]
        public decimal AmountPaid { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance owed must be greater than or equal to 0")]
        public decimal BalanceOwed { get; set; }

        [Required(ErrorMessage = "Accounts payable status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid accounts payable status")]
        public int AccountsPayableStatusId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid supplier")]
        public int SupplierId { get; set; }
    }
}
