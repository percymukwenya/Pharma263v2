using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Medicine
{
    public class AddMedicineDto
    {
        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Generic Name")]
        public string GenericName { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;

        [DisplayName("Dosage Form")]
        public string DosageForm { get; set; } = string.Empty;

        [DisplayName("Pack Size")]
        public string PackSize { get; set; } = string.Empty;

        [DisplayName("Quantity Per Unit")]
        public int QuantityPerUnit { get; set; }
    }
}
