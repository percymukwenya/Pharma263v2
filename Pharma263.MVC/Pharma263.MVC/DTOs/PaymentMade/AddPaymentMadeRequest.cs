using System;

namespace Pharma263.MVC.DTOs.PaymentMade
{
    public class AddPaymentMadeRequest
    {
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int AccountPayableId { get; set; }
        public int SupplierId { get; set; }
    }
}
