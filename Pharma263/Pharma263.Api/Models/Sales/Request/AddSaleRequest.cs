using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Sales.Request
{
    public class AddSaleRequest
    {
        [Required(ErrorMessage = "Sales date is required")]
        public DateTime SalesDate { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total must be greater than or equal to 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Sale status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid sale status")]
        public int SaleStatusId { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid payment method")]
        public int PaymentMethodId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
        public decimal Discount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Grand total must be greater than or equal to 0")]
        public decimal GrandTotal { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Sale items are required")]
        [MinLength(1, ErrorMessage = "At least one sale item is required")]
        public List<SaleItemModel> Items { get; set; }
    }
}
