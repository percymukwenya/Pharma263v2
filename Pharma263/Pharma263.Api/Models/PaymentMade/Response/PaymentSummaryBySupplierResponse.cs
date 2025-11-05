using System;

namespace Pharma263.Api.Models.PaymentMade.Response
{
    public class PaymentSummaryBySupplierResponse
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int PaymentCount { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public DateTime LastPaymentDate { get; set; }
    }
}
