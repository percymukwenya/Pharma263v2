using System;

namespace Pharma263.MVC.DTOs.PaymentReceived
{
    public class AddPaymentReceivedRequest
    {
        public decimal AmountReceived { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }
    }
}
