using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class Customer : ConcurrencyTokenEntity, IAuditable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhysicalAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string MCAZLicence { get; set; }
        public string HPALicense { get; set; }
        public string VATNumber { get; set; }

        public int CustomerTypeId { get; set; }
        public CustomerType CustomerType { get; set; }

        public Customer() : base()
        {
        }

        public Customer(string name, string email, string phone, string physicalAddress,
            string deliveryAddress, string mcazLisence,
            string vatNumber, int customerTypeId) : this()
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(physicalAddress, nameof(physicalAddress));

            Name = name;
            Email = email;
            Phone = phone;
            PhysicalAddress = physicalAddress;
            DeliveryAddress = deliveryAddress;
            MCAZLicence = mcazLisence;
            VATNumber = vatNumber;
            CustomerTypeId = customerTypeId;
        }

        public Customer(CustomerType customerType,string name, string email, string phone, string physicalAddress,
            string deliveryAddress, string mcazLisence,
            string vatNumber) : this()
        {
            ArgumentNullException.ThrowIfNull(customerType, nameof(customerType));
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(physicalAddress, nameof(physicalAddress));

            CustomerType = customerType;
            CustomerTypeId = customerType.Id;

            Name = name;
            Email = email;
            Phone = phone;
            PhysicalAddress = physicalAddress;
            DeliveryAddress = deliveryAddress;
            MCAZLicence = mcazLisence;
            VATNumber = vatNumber;
        }
    }
}
