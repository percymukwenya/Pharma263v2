namespace Pharma263.Integration.Api.Models.Request
{
    public class UpdateSupplierRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string MCAZLicence { get; set; }
        public string BusinessPartnerNumber { get; set; }
        public string VATNumber { get; set; }
    }
}
