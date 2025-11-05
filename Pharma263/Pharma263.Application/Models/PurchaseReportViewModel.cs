using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class PurchaseReportViewModel
    {
        public StoreSettingsDetailsDto Company { get; set; }
        public PurchaseDto Purchase { get; set; }
    }
}
