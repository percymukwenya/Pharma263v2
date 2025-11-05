using System.ComponentModel.DataAnnotations;
using System;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class QuotationDto
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset QuotationDate { get; set; }
        public string Notes { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Total { get; set; }

        public string QuotationStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Discount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double GrandTotal { get; set; }

        public string CustomerName { get; set; }
    }
}
