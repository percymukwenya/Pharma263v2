using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class PaymentMade : ConcurrencyTokenEntity, IAuditable
    {
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int AccountPayableId { get; set; }
        public AccountsPayable AccountPayable { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public PaymentMade() : base()
        {
        }

        public PaymentMade(decimal amountPaid, DateTime paymentDate, int paymentMethodId, int accountPayableId, int supplierId) : this()
        {
            AmountPaid = amountPaid;
            PaymentDate = paymentDate;
            PaymentMethodId = paymentMethodId;
            AccountPayableId = accountPayableId;
            SupplierId = supplierId;
        }

        public PaymentMade(PaymentMethod paymentMethod, AccountsPayable accountsPayable, Supplier supplier, decimal amountPaid, DateTime paymentDate) : this()
        {
            ArgumentNullException.ThrowIfNull(paymentMethod, nameof(paymentMethod));
            ArgumentNullException.ThrowIfNull(accountsPayable, nameof(accountsPayable));
            ArgumentNullException.ThrowIfNull(supplier, nameof(supplier));

            PaymentMethod = paymentMethod;
            PaymentMethodId = paymentMethod.Id;

            AccountPayable = accountsPayable;
            AccountPayableId = accountsPayable.Id;

            Supplier = supplier;
            SupplierId = supplier.Id;

            AmountPaid = amountPaid;
            PaymentDate = paymentDate;
        }
    }
}
