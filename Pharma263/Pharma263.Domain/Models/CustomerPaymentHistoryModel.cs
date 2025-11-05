using System;

namespace Pharma263.Domain.Models
{
    public class CustomerPaymentHistoryModel
    {
        public DateTime PaymentDate { get; set; }
        public double AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
    }
}
