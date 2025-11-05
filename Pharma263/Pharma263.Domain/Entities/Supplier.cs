using Pharma263.Domain.Common;

namespace Pharma263.Domain.Entities
{
    public class Supplier : ConcurrencyTokenEntity, IAuditable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string MCAZLicence { get; set; }
        public string BusinessPartnerNumber { get; set; }
        public string VATNumber { get; set; }

        public Supplier() : base()
        {
            
        }

        public Supplier(string name, string email, string phone, string address, 
            string notes, string mcazLicence, string vatNumber) : this()
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            Notes = notes;
            MCAZLicence = mcazLicence;
            VATNumber = vatNumber;
        }
    }
}
