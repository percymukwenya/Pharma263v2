using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class QuotationReportViewModel
    {
        public StoreSettingsDetailsDto Company { get; set; }
        public QuotationDto Quotation { get; set; }
    }
}
