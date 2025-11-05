using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharma263.Integration.Api.Models.Response
{
    public class CustomerStatementResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateRangeDto StatementPeriod { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<CustomerStatementTransactionDto> Transactions { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class SupplierStatementResponse
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateRangeDto StatementPeriod { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<SupplierStatementTransactionDto> Transactions { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class CustomerStatementTransactionDto
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceTable { get; set; }
        public decimal RunningBalance { get; set; }
    }

    public class SupplierStatementTransactionDto
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int ReferenceId { get; set; }
        public string ReferenceTable { get; set; }
        public decimal RunningBalance { get; set; }
    }

    public class DateRangeDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
