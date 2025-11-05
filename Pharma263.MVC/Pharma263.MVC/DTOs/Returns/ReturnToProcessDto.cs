using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Returns
{
    public class ReturnToProcessDto
    {
        public int SaleId { get; set; }
        public List<ReturnItemDto> ReturnItems { get; set; }

        public int ReturnReasonId { get; set; }
        public int ReturnDestinationId { get; set; }
    }
}
