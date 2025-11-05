using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class AccountsPayable : ConcurrencyTokenEntity, IAuditable
    {
        public decimal AmountOwed { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceOwed { get; set; }

        public int AccountsPayableStatusId { get; set; }
        public AccountsPayableStatus AccountsPayableStatus { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

    }
}
