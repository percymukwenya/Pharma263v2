using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Returns.Request
{
    public class SaleItemToReturnModel
    {
        [Required(ErrorMessage = "Sale item ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale item")]
        public int SaleItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Return destination is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid return destination")]
        public int ReturnDestinationId { get; set; }

        [Required(ErrorMessage = "Return reason is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid return reason")]
        public int ReturnReasonId { get; set; }

        [Required(ErrorMessage = "Stock ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid stock item")]
        public int StockId { get; set; }
    }
}
