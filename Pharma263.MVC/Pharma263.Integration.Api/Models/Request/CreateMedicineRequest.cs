namespace Pharma263.Integration.Api.Models.Request
{
    public class CreateMedicineRequest
    {
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string DosageForm { get; set; }
        public string PackSize { get; set; }
        public int QuantityPerUnit { get; set; }
    }
}
