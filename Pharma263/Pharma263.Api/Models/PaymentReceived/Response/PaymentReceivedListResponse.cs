using System;

namespace Pharma263.Api.Models.PaymentReceived.Response
{
    public class PaymentReceivedListResponse
    {
        public int Id { get; set; }
        public decimal AmountReceived { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int AccountsReceivableId { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
    }
}
