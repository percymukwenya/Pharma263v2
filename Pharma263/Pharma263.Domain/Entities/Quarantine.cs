using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class Quarantine : ConcurrencyTokenEntity
    {
        public int TotalQuantity { get; set; }
        public string BatchNo { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }

        public int ReturnReasonId { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public Quarantine() : base()
        {            
        }

        public Quarantine(int totalQuantity, int medicineId) : this()
        {
            TotalQuantity = totalQuantity;
            MedicineId = medicineId;
        }

        public Quarantine(Medicine medicine, int totalQuantity) : this()
        {
            Medicine = medicine;
            MedicineId = medicine.Id;

            TotalQuantity = totalQuantity;            
        }
    }
}
