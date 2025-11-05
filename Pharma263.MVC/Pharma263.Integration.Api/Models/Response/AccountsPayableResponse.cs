using System;

namespace Pharma263.Integration.Api.Models.Response
{
    public class AccountsPayableResponse
    {
        public int Id { get; set; }
        public decimal AmountOwed { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceOwed { get; set; }
        public string AccountsPayableStatus { get; set; }
        public string Supplier { get; set; }
        public int PurchaseId { get; set; }
    }
}
