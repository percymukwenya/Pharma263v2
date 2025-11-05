using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class SaleReportViewModel
    {
        public StoreSettingsDetailsDto Company { get; set; }
        public SaleDto Sale { get; set; }
    }
}
