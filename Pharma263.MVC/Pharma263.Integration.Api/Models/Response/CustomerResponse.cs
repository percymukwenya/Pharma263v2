namespace Pharma263.Integration.Api.Models.Response
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PhysicalAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string MCAZLicence { get; set; } = string.Empty;
        public string HPALicense { get; set; }
        public string VATNumber { get; set; }
        public int CustomerTypeId { get; set; }
    }
}
