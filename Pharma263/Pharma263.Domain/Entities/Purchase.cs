using Pharma263.Domain.Common;
using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Entities
{
    public class Purchase : ConcurrencyTokenEntity, IAuditable
    {
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int PurchaseStatusId { get; set; }
        public PurchaseStatus PurchaseStatus { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }


        public ICollection<PurchaseItems> Items { get; set; }

        public Purchase() : base()
        {
            Items = new List<PurchaseItems>();
        }

        public Purchase(DateTime purchaseDate, string notes, decimal total,
            decimal discount, decimal grandTotal,
            DateTime? paymentDueDate, int supplierId, int paymentMethodId, int purchaseStatusId) : this()
        {
            PurchaseDate = purchaseDate;
            Notes = notes;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
            PaymentDueDate = paymentDueDate;
            SupplierId = supplierId;
            PaymentMethodId = paymentMethodId;
            PurchaseStatusId = purchaseStatusId;
        }

        public Purchase(PaymentMethod paymentMethod, PurchaseStatus purchaseStatus, Supplier supplier, DateTime purchaseDate, string notes, decimal total,
            decimal discount, decimal grandTotal,
            DateTime? paymentDueDate) : this()
        {
            ArgumentNullException.ThrowIfNull(paymentMethod, nameof(paymentMethod));
            ArgumentNullException.ThrowIfNull(purchaseStatus, nameof(purchaseStatus));
            ArgumentNullException.ThrowIfNull(supplier, nameof(supplier));

            Supplier = supplier;
            SupplierId = supplier.Id;

            PaymentMethod = paymentMethod;
            PaymentMethodId = paymentMethod.Id;

            PurchaseStatus = purchaseStatus;
            PurchaseStatusId = purchaseStatus.Id;

            PurchaseDate = purchaseDate;
            Notes = notes;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
            PaymentDueDate = paymentDueDate;
        }
    }
}
