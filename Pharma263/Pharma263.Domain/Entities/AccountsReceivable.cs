using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class AccountsReceivable : ConcurrencyTokenEntity, IAuditable
    {
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceDue { get; set; }

        public int AccountsReceivableStatusId { get; set; }
        public AccountsReceivableStatus AccountsReceivableStatus { get; set; }        

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public AccountsReceivable() : base()
        {            
        }

        public AccountsReceivable(decimal amountDue, DateTime dueDate, decimal amountPaid, decimal balanceDue, int accountsReceivableStatusId, int customerId) : this()
        {
            AmountDue = amountDue;
            DueDate = dueDate;
            AmountPaid = amountPaid;
            BalanceDue = balanceDue;
            AccountsReceivableStatusId = accountsReceivableStatusId;
            CustomerId = customerId;
        }

        public AccountsReceivable(AccountsReceivableStatus accountsReceivableStatus, Customer customer, decimal amountDue, DateTime dueDate, decimal amountPaid, decimal balanceDue) : this()
        {
            ArgumentNullException.ThrowIfNull(accountsReceivableStatus, nameof(accountsReceivableStatus));
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            AccountsReceivableStatus = accountsReceivableStatus;
            AccountsReceivableStatusId = accountsReceivableStatus.Id;

            Customer = customer;
            CustomerId = customer.Id;

            AmountDue = amountDue;
            DueDate = dueDate;
            AmountPaid = amountPaid;
            BalanceDue = balanceDue;            
        }
    }
}
