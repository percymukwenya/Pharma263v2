using System;

namespace Pharma263.Api.Models.PaymentMade.Response
{
    public class PaymentMadeDetailsResponse
    {
        public int Id { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public int AccountPayableId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
