using System;

namespace Pharma263.MVC.Models
{
    public class AccountsReceivableReportViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public double AmountOwing { get; set; }
        public DateTime DueDate { get; set; }
    }
}
