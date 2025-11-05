using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class SupplierStatementViewModel
    {
        public StoreSettingsDetailsDto Company { get; set; }
        public SupplierStatementResponse Statement { get; set; }
    }
}
