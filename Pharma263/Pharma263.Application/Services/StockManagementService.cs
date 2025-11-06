using Microsoft.Extensions.Logging;
using Pharma263.Application.Contracts.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Pharma263.Application.Services
{
    /// <summary>
    /// Service for managing stock operations with business logic and validation
    /// </summary>
    public class StockManagementService : IStockManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockManagementService> _logger;

        public StockManagementService(
            IUnitOfWork unitOfWork,
            ILogger<StockManagementService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Adds quantity to stock with validation
        /// </summary>
        public async Task<ApiResponse<bool>> AddStockAsync(int stockId, int quantity, string reason)
        {
            try
            {
                // Validation
                if (quantity <= 0)
                {
                    return ApiResponse<bool>.CreateFailure(
                        "Quantity must be greater than zero",
                        (int)HttpStatusCode.BadRequest);
                }

                // Get stock item
                var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
                if (stock == null)
                {
                    return ApiResponse<bool>.CreateFailure(
                        $"Stock item with ID {stockId} not found",
                        (int)HttpStatusCode.NotFound);
                }

                // Store previous quantity for logging
                var previousQuantity = stock.TotalQuantity;

                // Apply business logic
                stock.TotalQuantity += quantity;

                // Update in database
                _unitOfWork.Repository<Stock>().Update(stock);

                // Log the operation
                _logger.LogInformation(
                    "Stock added: StockId={StockId}, Previous={Previous}, Added={Added}, New={New}, Reason={Reason}",
                    stockId, previousQuantity, quantity, stock.TotalQuantity, reason);

                return ApiResponse<bool>.CreateSuccess(
                    true,
                    $"Successfully added {quantity} units to stock",
                    (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock: StockId={StockId}, Quantity={Quantity}", stockId, quantity);
                return ApiResponse<bool>.CreateFailure(
                    $"Error adding stock: {ex.Message}",
                    (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deducts quantity from stock with validation
        /// </summary>
        public async Task<ApiResponse<bool>> DeductStockAsync(int stockId, int quantity, string reason)
        {
            try
            {
                // Validation
                if (quantity <= 0)
                {
                    return ApiResponse<bool>.CreateFailure(
                        "Quantity must be greater than zero",
                        (int)HttpStatusCode.BadRequest);
                }

                // Get stock item
                var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
                if (stock == null)
                {
                    return ApiResponse<bool>.CreateFailure(
                        $"Stock item with ID {stockId} not found",
                        (int)HttpStatusCode.NotFound);
                }

                // Check if sufficient stock is available
                if (stock.TotalQuantity < quantity)
                {
                    _logger.LogWarning(
                        "Insufficient stock: StockId={StockId}, Available={Available}, Requested={Requested}",
                        stockId, stock.TotalQuantity, quantity);

                    return ApiResponse<bool>.CreateFailure(
                        $"Insufficient stock. Available: {stock.TotalQuantity}, Requested: {quantity}",
                        (int)HttpStatusCode.BadRequest);
                }

                // Store previous quantity for logging
                var previousQuantity = stock.TotalQuantity;

                // Apply business logic
                stock.TotalQuantity -= quantity;

                // Check if stock is below notify threshold
                if (stock.TotalQuantity < stock.NotifyForQuantityBelow)
                {
                    _logger.LogWarning(
                        "Low stock warning: StockId={StockId}, Current={Current}, Threshold={Threshold}, MedicineId={MedicineId}",
                        stockId, stock.TotalQuantity, stock.NotifyForQuantityBelow, stock.MedicineId);

                    // TODO: Future enhancement - Send notification to admin/purchasing department
                }

                // Update in database
                _unitOfWork.Repository<Stock>().Update(stock);

                // Log the operation
                _logger.LogInformation(
                    "Stock deducted: StockId={StockId}, Previous={Previous}, Deducted={Deducted}, New={New}, Reason={Reason}",
                    stockId, previousQuantity, quantity, stock.TotalQuantity, reason);

                return ApiResponse<bool>.CreateSuccess(
                    true,
                    $"Successfully deducted {quantity} units from stock",
                    (int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting stock: StockId={StockId}, Quantity={Quantity}", stockId, quantity);
                return ApiResponse<bool>.CreateFailure(
                    $"Error deducting stock: {ex.Message}",
                    (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Checks if sufficient stock is available
        /// </summary>
        public async Task<bool> IsStockAvailableAsync(int stockId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
                if (stock == null)
                    return false;

                return stock.TotalQuantity >= quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking stock availability: StockId={StockId}, Quantity={Quantity}",
                    stockId, quantity);
                return false;
            }
        }

        /// <summary>
        /// Reserves stock for a pending operation (e.g., quotation)
        /// Future enhancement: Implement stock reservation logic
        /// </summary>
        public async Task<ApiResponse<bool>> ReserveStockAsync(int stockId, int quantity, string reservationId)
        {
            // TODO: Implement stock reservation logic
            // This would require a new StockReservation entity to track reserved quantities
            // For now, just check if stock is available

            var isAvailable = await IsStockAvailableAsync(stockId, quantity);
            if (!isAvailable)
            {
                return ApiResponse<bool>.CreateFailure(
                    "Insufficient stock for reservation",
                    (int)HttpStatusCode.BadRequest);
            }

            _logger.LogInformation(
                "Stock reservation requested (not yet implemented): StockId={StockId}, Quantity={Quantity}, ReservationId={ReservationId}",
                stockId, quantity, reservationId);

            return ApiResponse<bool>.CreateSuccess(
                true,
                "Stock reservation validated (full implementation pending)",
                (int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Releases a previously reserved stock
        /// Future enhancement: Implement reservation release logic
        /// </summary>
        public async Task<ApiResponse<bool>> ReleaseReservationAsync(string reservationId)
        {
            // TODO: Implement reservation release logic

            _logger.LogInformation(
                "Stock reservation release requested (not yet implemented): ReservationId={ReservationId}",
                reservationId);

            return await Task.FromResult(ApiResponse<bool>.CreateSuccess(
                true,
                "Stock reservation release validated (full implementation pending)",
                (int)HttpStatusCode.OK));
        }
    }
}
