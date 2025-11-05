using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.AccountsReceivable
{
    public class GetAccountsReceivableAgingDto
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Current (0-30 Days)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Current30Days { get; set; }

        [Display(Name = "31-60 Days")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Days31To60 { get; set; }

        [Display(Name = "61-90 Days")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Days61To90 { get; set; }

        [Display(Name = "Over 90 Days")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal DaysOver90 { get; set; }

        [Display(Name = "Total Balance")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalBalance { get; set; }
    }
}
