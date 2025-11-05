using System;

namespace Pharma263.Api.Models.PaymentReceived.Request
{
    public class UpdatePaymentReceivedRequest
    {
        public int Id { get; set; }
        public decimal AmountReceived { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }
    }
}
