using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class Medicine : ConcurrencyTokenEntity, IAuditable
    {
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string DosageForm { get; set; }
        public string PackSize { get; set; }
        public int QuantityPerUnit { get; set; }

        public Medicine() : base()
        {
        }

        public Medicine(string name, string genericname, string brand, string manufacturer,
            string dosageForm, string packSize, int quantityPerUnit) : this()
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(genericname, nameof(genericname));
            ArgumentNullException.ThrowIfNull(brand, nameof(brand));
            ArgumentNullException.ThrowIfNull(dosageForm, nameof(dosageForm));

            Name = name;
            GenericName = genericname;
            Brand = brand;
            Manufacturer = manufacturer;
            QuantityPerUnit = quantityPerUnit;
            DosageForm = dosageForm;
            PackSize = packSize;
        }
    }
}
