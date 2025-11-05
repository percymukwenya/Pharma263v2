using Pharma263.Domain.Common;
using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Entities
{
    public class Sales : ConcurrencyTokenEntity, IAuditable
    {
        public DateTime SalesDate { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int SaleStatusId { get; set; }
        public SaleStatus SaleStatus { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<SalesItems> Items { get; set; }

        public Sales() : base()
        {
            Items = new List<SalesItems>();
        }

        public Sales(DateTime salesDate, string notes, decimal total, decimal discount, decimal grandTotal,
            DateTime? paymentDueDate, int paymentMethodId, int saleStatusId, int customerId) : this()
        {
            SalesDate = salesDate;
            Notes = notes;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
            PaymentDueDate = paymentDueDate;
            PaymentMethodId = paymentMethodId;
            SaleStatusId = saleStatusId;
            CustomerId = customerId;
        }

        public Sales(PaymentMethod paymentMethod, SaleStatus saleStatus, Customer customer, DateTime salesDate, string notes, decimal total, decimal discount, decimal grandTotal,
            DateTime? paymentDueDate) : this()
        {
            PaymentMethod = paymentMethod;
            PaymentMethodId = paymentMethod.Id;

            SaleStatus = saleStatus;
            SaleStatusId = saleStatus.Id;

            Customer = customer;
            CustomerId = customer.Id;

            SalesDate = salesDate;
            Notes = notes;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
            PaymentDueDate = paymentDueDate;
        }
    }
}
