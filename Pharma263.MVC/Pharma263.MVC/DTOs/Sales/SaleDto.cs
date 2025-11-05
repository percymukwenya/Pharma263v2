using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Sales
{
    public class SaleDto
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset SalesDate { get; set; }
        public string Notes { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Total { get; set; }
        public string SaleStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Discount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double GrandTotal { get; set; }
        public string PaymentMethod { get; set; }

        public string CustomerName { get; set; }
    }
}
