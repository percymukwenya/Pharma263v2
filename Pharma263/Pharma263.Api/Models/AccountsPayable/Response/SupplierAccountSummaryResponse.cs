using System;

namespace Pharma263.Api.Models.AccountsPayable.Response
{
    public class SupplierAccountSummaryResponse
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmountOwed { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalBalanceOwed { get; set; }
        public int NumberOfAccounts { get; set; }
        public DateTime? EarliestDueDate { get; set; }
    }
}
