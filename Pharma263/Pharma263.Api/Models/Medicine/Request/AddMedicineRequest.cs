using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Medicine.Request
{
    public class AddMedicineRequest
    {
        [Required(ErrorMessage = "Medicine name is required")]
        [StringLength(100, ErrorMessage = "Medicine name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Generic name is required")]
        [StringLength(100, ErrorMessage = "Generic name cannot exceed 100 characters")]
        public string GenericName { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string Brand { get; set; }

        [StringLength(100, ErrorMessage = "Manufacturer cannot exceed 100 characters")]
        public string Manufacturer { get; set; }

        [StringLength(50, ErrorMessage = "Dosage form cannot exceed 50 characters")]
        public string DosageForm { get; set; }

        [StringLength(50, ErrorMessage = "Pack size cannot exceed 50 characters")]
        public string PackSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity per unit must be greater than 0")]
        public int QuantityPerUnit { get; set; }
    }
}
