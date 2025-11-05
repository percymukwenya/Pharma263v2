using System;

namespace Pharma263.Api.Models.PaymentReceived.Response
{
    public class PaymentSummaryByCustomerResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int PaymentCount { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public DateTime LastPaymentDate { get; set; }
    }
}
