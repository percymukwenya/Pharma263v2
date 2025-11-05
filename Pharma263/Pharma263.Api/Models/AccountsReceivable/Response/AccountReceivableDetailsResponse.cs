using System;

namespace Pharma263.Api.Models.AccountsReceivable.Response
{
    public class AccountReceivableDetailsResponse
    {
        public int Id { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceDue { get; set; }
        public string AccountsReceivableStatus { get; set; }
        public string Customer { get; set; }
        public  int CustomerId { get; set; }
    }
}
