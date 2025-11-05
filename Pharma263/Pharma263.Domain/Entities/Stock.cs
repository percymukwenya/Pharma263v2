using Pharma263.Domain.Common;
using System;

namespace Pharma263.Domain.Entities
{
    public class Stock : ConcurrencyTokenEntity, IAuditable
    {
        public int TotalQuantity { get; set; }
        public int NotifyForQuantityBelow { get; set; }        
        public string BatchNo { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public Stock() : base()
        {            
        }

        public Stock(int totalQuantity, int notifyForQuantityBelow, string batchNo, DateTimeOffset expiryDate,
            decimal buyingPrice, decimal sellingPrice, int medicineId) : this()
        {
            TotalQuantity = totalQuantity;
            NotifyForQuantityBelow = notifyForQuantityBelow;            
            BatchNo = batchNo;
            ExpiryDate = expiryDate;
            BuyingPrice = buyingPrice;
            SellingPrice = sellingPrice;
            MedicineId = medicineId;
        }

        public Stock(int totalQuantity, int notifyForQuantityBelow, decimal buyingPrice, 
            decimal sellingPrice, int medicineId) : this()
        {
            TotalQuantity = totalQuantity;
            NotifyForQuantityBelow = notifyForQuantityBelow;
            BuyingPrice = buyingPrice;
            SellingPrice = sellingPrice;
            MedicineId = medicineId;
        }

        public Stock(Medicine medicine, int totalQuantity, int notifyForQuantityBelow, string batchNo, DateTimeOffset expiryDate,
            decimal buyingPrice, decimal sellingPrice) : this()
        {
            Medicine = medicine;
            MedicineId = medicine.Id;

            TotalQuantity = totalQuantity;
            NotifyForQuantityBelow = notifyForQuantityBelow;
            BatchNo = batchNo;
            ExpiryDate = expiryDate;
            BuyingPrice = buyingPrice;
            SellingPrice = sellingPrice;            
        }
    }
}
