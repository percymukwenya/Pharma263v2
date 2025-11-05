using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Returns.Request
{
    public class ProcessReturnRequest
    {
        [Required(ErrorMessage = "Sale ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Sale items to return are required")]
        [MinLength(1, ErrorMessage = "At least one sale item must be selected for return")]
        public List<SaleItemToReturnModel> SaleItemsToReturn { get; set; }
    }
}
