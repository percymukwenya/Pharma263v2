using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class CustomerStatementViewModel
    {
        public StoreSettingsDetailsDto Company { get; set; }
        public CustomerStatementResponse Statement { get; set; }
    }
}
