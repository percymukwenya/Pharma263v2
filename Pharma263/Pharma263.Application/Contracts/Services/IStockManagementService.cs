using Pharma263.Domain.Common;
using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Services
{
    /// <summary>
    /// Service for managing stock operations with business logic and validation
    /// </summary>
    public interface IStockManagementService
    {
        /// <summary>
        /// Adds quantity to stock with validation
        /// </summary>
        /// <param name="stockId">Stock ID</param>
        /// <param name="quantity">Quantity to add (must be positive)</param>
        /// <param name="reason">Reason for stock addition (for audit trail)</param>
        /// <returns>ApiResponse indicating success or failure</returns>
        Task<ApiResponse<bool>> AddStockAsync(int stockId, int quantity, string reason);

        /// <summary>
        /// Deducts quantity from stock with validation
        /// </summary>
        /// <param name="stockId">Stock ID</param>
        /// <param name="quantity">Quantity to deduct (must be positive)</param>
        /// <param name="reason">Reason for stock deduction (for audit trail)</param>
        /// <returns>ApiResponse indicating success or failure</returns>
        Task<ApiResponse<bool>> DeductStockAsync(int stockId, int quantity, string reason);

        /// <summary>
        /// Checks if sufficient stock is available
        /// </summary>
        /// <param name="stockId">Stock ID</param>
        /// <param name="quantity">Quantity needed</param>
        /// <returns>True if stock is available, false otherwise</returns>
        Task<bool> IsStockAvailableAsync(int stockId, int quantity);

        /// <summary>
        /// Reserves stock for a pending operation (e.g., quotation)
        /// </summary>
        /// <param name="stockId">Stock ID</param>
        /// <param name="quantity">Quantity to reserve</param>
        /// <param name="reservationId">Unique ID for the reservation</param>
        /// <returns>ApiResponse indicating success or failure</returns>
        Task<ApiResponse<bool>> ReserveStockAsync(int stockId, int quantity, string reservationId);

        /// <summary>
        /// Releases a previously reserved stock
        /// </summary>
        /// <param name="reservationId">Reservation ID to release</param>
        /// <returns>ApiResponse indicating success or failure</returns>
        Task<ApiResponse<bool>> ReleaseReservationAsync(string reservationId);
    }
}
