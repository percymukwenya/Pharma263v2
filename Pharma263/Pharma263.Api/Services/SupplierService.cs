using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Models.Supplier.Request;
using Pharma263.Api.Models.Supplier.Response;
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
    public class SupplierService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<SupplierService> _logger;
        private readonly IMemoryCache _cache;

        public SupplierService(IUnitOfWork unitOfWork, IAppLogger<SupplierService> logger, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<List<SupplierListResponse>>> GetSuppliers()
        {
            const string cacheKey = "suppliers_all";

            try
            {
                // Try to get from cache first
                if (_cache.TryGetValue(cacheKey, out List<SupplierListResponse> cachedSuppliers))
                {
                    return ApiResponse<List<SupplierListResponse>>.CreateSuccess(cachedSuppliers);
                }

                var suppliers = await _unitOfWork.Repository<Supplier>().GetAllAsync();

                var mappedSuppliers = suppliers.Select(x => new SupplierListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Notes = x.Notes,
                    MCAZLicence = x.MCAZLicence,
                    BusinessPartnerNumber = x.BusinessPartnerNumber,
                    VATNumber = x.VATNumber
                }).ToList();

                // Cache for 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set(cacheKey, mappedSuppliers, cacheEntryOptions);

                return ApiResponse<List<SupplierListResponse>>.CreateSuccess(mappedSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Suppliers", JsonSerializer.Serialize(ex));

                return ApiResponse<List<SupplierListResponse>>.CreateFailure($"Failed to retrieve suppliers. {ex.Message}", 500);
            }
        }

        private void InvalidateSupplierCache()
        {
            _cache.Remove("suppliers_all");
        }

        public async Task<ApiResponse<PaginatedList<SupplierListResponse>>> GetSuppliersPaged(PagedRequest request)
        {
            try
            {
                // Build filter expression
                Expression<Func<Supplier, bool>> filter = x => !x.IsDeleted;
                
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => !x.IsDeleted && (x.Name.ToLower().Contains(searchTerm) ||
                                                  x.Email.ToLower().Contains(searchTerm) ||
                                                  x.Phone.Contains(searchTerm) ||
                                                  x.Address.ToLower().Contains(searchTerm));
                }

                // Build sorting
                Func<IQueryable<Supplier>, IOrderedQueryable<Supplier>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(request.SortBy))
                {
                    switch (request.SortBy.ToLower())
                    {
                        case "name":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Name) : q => q.OrderBy(x => x.Name);
                            break;
                        case "email":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Email) : q => q.OrderBy(x => x.Email);
                            break;
                        case "phone":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Phone) : q => q.OrderBy(x => x.Phone);
                            break;
                        case "address":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Address) : q => q.OrderBy(x => x.Address);
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
                var paginatedSuppliers = await _unitOfWork.Repository<Supplier>()
                    .GetPaginatedAsync(request.Page, request.PageSize, filter, orderBy);

                var mappedSuppliers = paginatedSuppliers.Items.Select(x => new SupplierListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Notes = x.Notes,
                    MCAZLicence = x.MCAZLicence,
                    BusinessPartnerNumber = x.BusinessPartnerNumber,
                    VATNumber = x.VATNumber
                }).ToList();

                var result = new PaginatedList<SupplierListResponse>(
                    mappedSuppliers, paginatedSuppliers.TotalCount, paginatedSuppliers.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<SupplierListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving paginated suppliers", JsonSerializer.Serialize(ex));
                return ApiResponse<PaginatedList<SupplierListResponse>>.CreateFailure($"Failed to retrieve suppliers. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SupplierDetailsResponse>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id);

                if (supplier == null) return ApiResponse<SupplierDetailsResponse>.CreateFailure("Supplier not found", 404);

                var mappedSupplier = new SupplierDetailsResponse
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Email = supplier.Email,
                    Phone = supplier.Phone,
                    Address = supplier.Address,
                    Notes = supplier.Notes,
                    MCAZLicence = supplier.MCAZLicence,
                    BusinessPartnerNumber = supplier.BusinessPartnerNumber,
                    VATNumber = supplier.VATNumber
                };

                return ApiResponse<SupplierDetailsResponse>.CreateSuccess(mappedSupplier);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Supplier", JsonSerializer.Serialize(ex));

                return ApiResponse<SupplierDetailsResponse>.CreateFailure($"Failed to retrieve supplier. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<bool>> AddSupplier(AddSupplierRequest request)
        {
            try
            {
                var supplierToCreate = new Supplier
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    Notes = request.Notes,
                    MCAZLicence = request.MCAZLicence,
                    BusinessPartnerNumber = request.BusinessPartnerNumber,
                    VATNumber = request.VATNumber
                };

                await _unitOfWork.Repository<Supplier>().AddAsync(supplierToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error creating supplier");

                // Invalidate cache after successful creation
                InvalidateSupplierCache();

                return ApiResponse<bool>.CreateSuccess(true, "Supplier created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error updating Supplier", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to update Supplier", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSupplier(UpdateSupplierRequest request)
        {
            try
            {
                var existingSupplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(request.Id);

                if (existingSupplier == null) return ApiResponse<bool>.CreateFailure("Supplier not found", 404);

                existingSupplier.Name = request.Name;
                existingSupplier.Email = request.Email;
                existingSupplier.Phone = request.Phone;
                existingSupplier.Address = request.Address;
                existingSupplier.Notes = request.Notes;
                existingSupplier.MCAZLicence = request.MCAZLicence;
                existingSupplier.BusinessPartnerNumber = request.BusinessPartnerNumber;
                existingSupplier.VATNumber = request.VATNumber;

                _unitOfWork.Repository<Supplier>().Update(existingSupplier);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error updating supplier");

                // Invalidate cache after successful update
                InvalidateSupplierCache();

                return ApiResponse<bool>.CreateSuccess(true, "Supplier updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error updating Supplier", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to update Supplier", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSupplier(int id)
        {
            try
            {
                var supplierToDelete = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id);

                if (supplierToDelete == null) return ApiResponse<bool>.CreateFailure("Supplier not found", 404);

                _unitOfWork.Repository<Supplier>().Delete(supplierToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error deleting Supplier");

                // Invalidate cache after successful deletion
                InvalidateSupplierCache();

                return ApiResponse<bool>.CreateSuccess(true, "Supplier deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error deleting Supplier", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to delete supplier", 500);
            }
        }
    }
}
