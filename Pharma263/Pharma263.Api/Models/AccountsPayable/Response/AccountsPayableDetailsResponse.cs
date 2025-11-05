using System;

namespace Pharma263.Api.Models.AccountsPayable.Response
{
    public class AccountsPayableDetailsResponse
    {
        public decimal AmountOwed { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceOwed { get; set; }
        public string AccountsPayableStatus { get; set; }
        public int AccountsPayableStatusId { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
    }
}
