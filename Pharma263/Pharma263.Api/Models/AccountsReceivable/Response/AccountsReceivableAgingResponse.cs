namespace Pharma263.Api.Models.AccountsReceivable.Response
{
    public class AccountsReceivableAgingResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Current30Days { get; set; }
        public decimal Days31To60 { get; set; }
        public decimal Days61To90 { get; set; }
        public decimal DaysOver90 { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
