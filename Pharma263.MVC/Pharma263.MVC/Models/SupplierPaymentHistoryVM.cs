using System;

namespace Pharma263.MVC.Models
{
    public class SupplierPaymentHistoryVM
    {
        public DateTime PaymentDate { get; set; }
        public double AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
    }
}
