using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Returns
{
    public class ProcessReturnRequestDto
    {
        public int SaleId { get; set; }
        public List<SaleItemToReturnDto> SaleItemsToReturn { get; set; } = new List<SaleItemToReturnDto>();
    }
}
