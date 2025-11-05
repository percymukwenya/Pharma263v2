using System;

namespace Pharma263.MVC.Models
{
    public class AccountsPayableReportViewModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public double AmountOwing { get; set; }
        public DateTime DueDate { get; set; }
    }
}
