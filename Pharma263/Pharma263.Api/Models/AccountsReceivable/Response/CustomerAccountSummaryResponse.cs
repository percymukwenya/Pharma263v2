using System;

namespace Pharma263.Api.Models.AccountsReceivable.Response
{
    public class CustomerAccountSummaryResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalBalanceDue { get; set; }
        public int NumberOfAccounts { get; set; }
        public DateTime? EarliestDueDate { get; set; }
    }
}
