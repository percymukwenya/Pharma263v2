using System;

namespace Pharma263.MVC.Exceptions
{
    /// <summary>
    /// Exception thrown when there is insufficient stock to fulfill an operation
    /// Example: Cannot sell 100 units when only 50 available
    /// </summary>
    public class InsufficientStockException : Exception
    {
        public int MedicineId { get; }
        public string MedicineName { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientStockException(
            int medicineId,
            string medicineName,
            int requestedQuantity,
            int availableQuantity)
            : base($"Insufficient stock for '{medicineName}'. Requested: {requestedQuantity}, Available: {availableQuantity}")
        {
            MedicineId = medicineId;
            MedicineName = medicineName;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }

        public InsufficientStockException(string message)
            : base(message)
        {
        }

        public InsufficientStockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
