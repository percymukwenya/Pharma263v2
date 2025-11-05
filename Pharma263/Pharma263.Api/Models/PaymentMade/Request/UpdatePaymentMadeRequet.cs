using Pharma263.Domain.Entities;
using System;

namespace Pharma263.Api.Models.PaymentMade.Request
{
    public class UpdatePaymentMadeRequet
    {
        public int Id { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int AccountPayableId { get; set; }
        public int SupplierId { get; set; }
    }
}
