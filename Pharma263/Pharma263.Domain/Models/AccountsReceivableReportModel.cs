using System;

namespace Pharma263.Domain.Models
{
    public class AccountsReceivableReportModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public double AmountDue { get; set; }
        public double AmountPaid { get; set; }
        public double BalanceDue { get; set; }
        public DateTime DueDate { get; set; }
    }
}
