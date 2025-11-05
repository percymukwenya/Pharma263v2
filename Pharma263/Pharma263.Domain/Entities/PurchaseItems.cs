using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class PurchaseItems : ConcurrencyTokenEntity, IAuditable
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string BatchNo { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public PurchaseItems() : base()
        {
        }

        public PurchaseItems(decimal price, int quantity, decimal amount, int medicineId, string batchNo, int purchaseId) : this()
        {
            Price = price;
            Quantity = quantity;
            Amount = amount;
            MedicineId = medicineId;
            BatchNo = batchNo;
            PurchaseId = purchaseId;
        }

        public PurchaseItems(Medicine medicine, Purchase purchase, decimal price, int quantity, decimal amount, string batchNo) : this()
        {
            ArgumentNullException.ThrowIfNull(medicine, nameof(medicine));
            ArgumentNullException.ThrowIfNull(purchase, nameof(purchase));

            Medicine = medicine;
            MedicineId = medicine.Id;

            Purchase = purchase;
            PurchaseId = purchase.Id;

            Price = price;
            Quantity = quantity;
            Amount = amount;
            BatchNo = batchNo;

        }
    }
}
