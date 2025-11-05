using System;

namespace Pharma263.Integration.Api.Models.Response
{
    public class MedicineDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string GenericName { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string DosageForm { get; set; }
        public string PackSize { get; set; }
        public int QuantityPerUnit { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
