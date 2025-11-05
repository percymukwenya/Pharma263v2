using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class PaymentReceived : ConcurrencyTokenEntity, IAuditable
    {
        public decimal AmountReceived { get; set; }
        public DateTime PaymentDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int AccountsReceivableId { get; set; }
        public AccountsReceivable AccountsReceivable { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public PaymentReceived() : base()
        {
        }

        public PaymentReceived(decimal amountReceived, DateTime paymentDate, int paymentMethodId, int accountsReceivableId, int customerId) : this()
        {
            AmountReceived = amountReceived;
            PaymentDate = paymentDate;
            PaymentMethodId = paymentMethodId;
            AccountsReceivableId = accountsReceivableId;
            CustomerId = customerId;
        }

        public PaymentReceived(PaymentMethod paymentMethod, AccountsReceivable accountsReceivable, Customer customer, decimal amountReceived, DateTime paymentDate) : this()
        {
            ArgumentNullException.ThrowIfNull(paymentMethod, nameof(paymentMethod));
            ArgumentNullException.ThrowIfNull(accountsReceivable, nameof(accountsReceivable));
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            PaymentMethod = paymentMethod;
            PaymentMethodId = paymentMethod.Id;

            AccountsReceivable = accountsReceivable;
            AccountsReceivableId = accountsReceivable.Id;

            Customer = customer;
            CustomerId = customer.Id;

            AmountReceived = amountReceived;
            PaymentDate = paymentDate;
        }
    }
}
