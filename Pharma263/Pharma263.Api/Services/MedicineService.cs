using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Models.Medicine.Request;
using Pharma263.Api.Models.Medicine.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class MedicineService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<MedicineService> _logger;
        private readonly IMemoryCache _cache;

        public MedicineService(IUnitOfWork unitOfWork, IAppLogger<MedicineService> logger, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<List<MedicineListResponse>>> GetMedicines()
        {
            const string cacheKey = "medicines_all";

            try
            {
                // Try to get from cache first
                if (_cache.TryGetValue(cacheKey, out List<MedicineListResponse> cachedMedicines))
                {
                    return ApiResponse<List<MedicineListResponse>>.CreateSuccess(cachedMedicines);
                }

                var medicines = await _unitOfWork.Repository<Medicine>().GetAllAsync();

                var mappedMedicines = medicines.Select(x => new MedicineListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    GenericName = x.GenericName,
                    Brand = x.Brand,
                    Manufacturer = x.Manufacturer,
                    DosageForm = x.DosageForm,
                    PackSize = x.PackSize,
                    QuantityPerUnit = x.QuantityPerUnit
                }).ToList();

                // Cache for 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set(cacheKey, mappedMedicines, cacheEntryOptions);

                return ApiResponse<List<MedicineListResponse>>.CreateSuccess(mappedMedicines);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Medicines", JsonSerializer.Serialize(ex));

                return ApiResponse<List<MedicineListResponse>>.CreateFailure($"Failed to retrieve medicines. {ex.Message}", 500);
            }
        }

        private void InvalidateMedicineCache()
        {
            _cache.Remove("medicines_all");
        }

        public async Task<ApiResponse<PaginatedList<MedicineListResponse>>> GetMedicinesPaged(PagedRequest request)
        {
            try
            {
                // Build filter expression
                Expression<Func<Medicine, bool>> filter = x => !x.IsDeleted;
                
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => !x.IsDeleted && (x.Name.ToLower().Contains(searchTerm) ||
                                                  x.GenericName.ToLower().Contains(searchTerm) ||
                                                  x.Brand.ToLower().Contains(searchTerm) ||
                                                  x.Manufacturer.ToLower().Contains(searchTerm));
                }

                // Build sorting
                Func<IQueryable<Medicine>, IOrderedQueryable<Medicine>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(request.SortBy))
                {
                    switch (request.SortBy.ToLower())
                    {
                        case "name":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Name) : q => q.OrderBy(x => x.Name);
                            break;
                        case "genericname":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.GenericName) : q => q.OrderBy(x => x.GenericName);
                            break;
                        case "brand":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Brand) : q => q.OrderBy(x => x.Brand);
                            break;
                        case "manufacturer":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Manufacturer) : q => q.OrderBy(x => x.Manufacturer);
                            break;
                        case "createddate":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.CreatedDate) : q => q.OrderBy(x => x.CreatedDate);
                            break;
                        default:
                            orderBy = q => q.OrderBy(x => x.Name);
                            break;
                    }
                }
                else
                {
                    orderBy = q => q.OrderBy(x => x.Name);
                }

                // Get paginated results using existing method
                var paginatedMedicines = await _unitOfWork.Repository<Medicine>()
                    .GetPaginatedAsync(request.Page, request.PageSize, filter, orderBy);

                var mappedMedicines = paginatedMedicines.Items.Select(x => new MedicineListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    GenericName = x.GenericName,
                    Brand = x.Brand,
                    Manufacturer = x.Manufacturer,
                    DosageForm = x.DosageForm,
                    PackSize = x.PackSize,
                    QuantityPerUnit = x.QuantityPerUnit
                }).ToList();

                var result = new PaginatedList<MedicineListResponse>(
                    mappedMedicines, paginatedMedicines.TotalCount, paginatedMedicines.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<MedicineListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving paginated medicines", JsonSerializer.Serialize(ex));
                return ApiResponse<PaginatedList<MedicineListResponse>>.CreateFailure($"Failed to retrieve medicines. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<MedicineDetailsResponse>> GetMedicine(int id)
        {
            try
            {
                var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(id);

                if (medicine == null) return ApiResponse<MedicineDetailsResponse>.CreateFailure("Medicine not found", 404);

                var mappedMedicine = new MedicineDetailsResponse
                {
                    Id = medicine.Id,
                    Name = medicine.Name,
                    GenericName = medicine.GenericName,
                    Brand = medicine.Brand,
                    Manufacturer = medicine.Manufacturer,
                    DosageForm = medicine.DosageForm,
                    PackSize = medicine.PackSize,
                    QuantityPerUnit = medicine.QuantityPerUnit
                };

                return ApiResponse<MedicineDetailsResponse>.CreateSuccess(mappedMedicine);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Medicine", JsonSerializer.Serialize(ex));

                return ApiResponse<MedicineDetailsResponse>.CreateFailure($"Failed to retrieve medicine. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> AddMedicine(AddMedicineRequest request)
        {
            try
            {
                var medicineToCreate = new Medicine
                {
                    Name = request.Name,
                    GenericName = request.GenericName,
                    Brand = request.Brand,
                    Manufacturer = request.Manufacturer,
                    DosageForm = request.DosageForm,
                    PackSize = request.PackSize,
                    QuantityPerUnit = request.QuantityPerUnit
                };

                await _unitOfWork.Repository<Medicine>().AddAsync(medicineToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error creating Medicine");

                // Invalidate cache after successful creation
                InvalidateMedicineCache();

                return ApiResponse<bool>.CreateSuccess(true, "Medicine created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error adding Medicine", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to create Medicine", 500);
            }

        }

        public async Task<ApiResponse<bool>> UpdateMedicine(UpdateMedicineRequest request)
        {
            try
            {
                var existingMedicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(request.Id);

                if (existingMedicine == null) return ApiResponse<bool>.CreateFailure("Medicine not found", 404);

                existingMedicine.Name = request.Name;
                existingMedicine.GenericName = request.GenericName;
                existingMedicine.Brand = request.Brand;
                existingMedicine.Manufacturer = request.Manufacturer;
                existingMedicine.DosageForm = request.DosageForm;
                existingMedicine.PackSize = request.PackSize;
                existingMedicine.QuantityPerUnit = request.QuantityPerUnit;

                _unitOfWork.Repository<Medicine>().Update(existingMedicine);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error updating medicine");

                // Invalidate cache after successful update
                InvalidateMedicineCache();

                return ApiResponse<bool>.CreateSuccess(true, "Medicine updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error updating Medicine", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to update Medicine", 500);
            }

        }

        public async Task<ApiResponse<bool>> DeleteMedicine(int id)
        {
            try
            {
                var medicineToDelete = await _unitOfWork.Repository<Medicine>().GetByIdAsync(id);

                if (medicineToDelete == null) return ApiResponse<bool>.CreateFailure("Medicine not found", 404);

                _unitOfWork.Repository<Medicine>().Delete(medicineToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error deleting Medicine");

                // Invalidate cache after successful deletion
                InvalidateMedicineCache();

                return ApiResponse<bool>.CreateSuccess(true, "Medicine deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error deleting Medicine", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to delete medicine", 500);
            }
        }
    }
}
