using System;

namespace Pharma263.MVC.DTOs.Medicine
{
    public class MedicineDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string GenericName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string DosageForm { get; set; } = string.Empty;
        public string PackSize { get; set; } = string.Empty;        
        public int QuantityPerUnit { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
