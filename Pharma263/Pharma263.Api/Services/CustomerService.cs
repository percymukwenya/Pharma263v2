using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Models.Customer.Request;
using Pharma263.Api.Models.Customer.Response;
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
    public class CustomerService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<CustomerService> _logger;
        private readonly IMemoryCache _cache;

        public CustomerService(IUnitOfWork unitOfWork, IAppLogger<CustomerService> logger, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<List<CustomerListResponse>>> GetCustomers()
        {
            const string cacheKey = "customers_all";

            try
            {
                // Try to get from cache first
                if (_cache.TryGetValue(cacheKey, out List<CustomerListResponse> cachedCustomers))
                {
                    return ApiResponse<List<CustomerListResponse>>.CreateSuccess(cachedCustomers);
                }

                var customers = await _unitOfWork.Repository<Customer>().GetAllAsync();

                var mappedCustomers = customers.Select(x => new CustomerListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    PhysicalAddress = x.PhysicalAddress,
                    DeliveryAddress = x.DeliveryAddress,
                    MCAZLicence = x.MCAZLicence,
                    HPALicense = x.HPALicense,
                    VATNumber = x.VATNumber,
                    CustomerTypeId = x.CustomerTypeId
                }).ToList();

                // Cache for 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set(cacheKey, mappedCustomers, cacheEntryOptions);

                return ApiResponse<List<CustomerListResponse>>.CreateSuccess(mappedCustomers);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Customers", JsonSerializer.Serialize(ex));

                return ApiResponse<List<CustomerListResponse>>.CreateFailure($"Failed to retrieve customers. {ex.Message}", 500);
            }

        }

        private void InvalidateCustomerCache()
        {
            _cache.Remove("customers_all");
            // Remove any paginated cache entries - for now we'll clear the specific key
            // In a more sophisticated implementation, you might want to track all related cache keys
        }

        public async Task<ApiResponse<PaginatedList<CustomerListResponse>>> GetCustomersPaged(PagedRequest request)
        {
            try
            {
                // Build filter expression
                Expression<Func<Customer, bool>> filter = x => !x.IsDeleted;
                
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => !x.IsDeleted && (x.Name.ToLower().Contains(searchTerm) ||
                                                  x.Email.ToLower().Contains(searchTerm) ||
                                                  x.Phone.Contains(searchTerm));
                }

                // Build sorting
                Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy = null;
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
                var paginatedCustomers = await _unitOfWork.Repository<Customer>()
                    .GetPaginatedAsync(request.Page, request.PageSize, filter, orderBy);

                var mappedCustomers = paginatedCustomers.Items.Select(x => new CustomerListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    PhysicalAddress = x.PhysicalAddress,
                    DeliveryAddress = x.DeliveryAddress,
                    MCAZLicence = x.MCAZLicence,
                    HPALicense = x.HPALicense,
                    VATNumber = x.VATNumber,
                    CustomerTypeId = x.CustomerTypeId
                }).ToList();

                var result = new PaginatedList<CustomerListResponse>(
                    mappedCustomers, paginatedCustomers.TotalCount, paginatedCustomers.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<CustomerListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving paginated customers", JsonSerializer.Serialize(ex));
                return ApiResponse<PaginatedList<CustomerListResponse>>.CreateFailure($"Failed to retrieve customers. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<CustomerDetailsResponse>> GetCustomer(int id)
        {
            try
            {
                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

                if (customer == null) return ApiResponse<CustomerDetailsResponse>.CreateFailure("Customer not found", 404);

                var mappedCustomer = new CustomerDetailsResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    PhysicalAddress = customer.PhysicalAddress,
                    DeliveryAddress = customer.DeliveryAddress,
                    MCAZLicence = customer.MCAZLicence,
                    HPALicense = customer.HPALicense,
                    VATNumber = customer.VATNumber,
                    CustomerTypeId = customer.CustomerTypeId
                };

                return ApiResponse<CustomerDetailsResponse>.CreateSuccess(mappedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving Customer", JsonSerializer.Serialize(ex));

                return ApiResponse<CustomerDetailsResponse>.CreateFailure($"Failed to retrieve customer. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> AddCustomer(AddCustomerRequest request)
        {
            try
            {
                var customerToCreate = new Customer
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    PhysicalAddress = request.PhysicalAddress,
                    DeliveryAddress = request.DeliveryAddress,
                    MCAZLicence = request.MCAZLicence,
                    HPALicense = request.HPALicense,
                    VATNumber = request.VATNumber,
                    CustomerTypeId = request.CustomerTypeId
                };

                await _unitOfWork.Repository<Customer>().AddAsync(customerToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error creating customer");

                // Invalidate cache after successful creation
                InvalidateCustomerCache();

                return ApiResponse<bool>.CreateSuccess(true, "Customer created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error adding Customer", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to create customer", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateCustomer(UpdateCustomerRequest request)
        {
            try
            {
                var existingCustomer = await _unitOfWork.Repository<Customer>().GetByIdAsync(request.Id);

                if (existingCustomer == null) return ApiResponse<bool>.CreateFailure("Customer not found", 404);

                existingCustomer.Name = request.Name;
                existingCustomer.Email = request.Email;
                existingCustomer.Phone = request.Phone;
                existingCustomer.PhysicalAddress = request.PhysicalAddress;
                existingCustomer.DeliveryAddress = request.DeliveryAddress;
                existingCustomer.MCAZLicence = request.MCAZLicence;
                existingCustomer.HPALicense = request.HPALicense;
                existingCustomer.VATNumber = request.VATNumber;
                existingCustomer.CustomerTypeId = request.CustomerTypeId;

                _unitOfWork.Repository<Customer>().Update(existingCustomer);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error updating customer");

                // Invalidate cache after successful update
                InvalidateCustomerCache();

                return ApiResponse<bool>.CreateSuccess(true, "Customer updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error updating Customer", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to update customer", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteCustomer(int id)
        {
            try
            {
                var customerToDelete = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

                if (customerToDelete == null) return ApiResponse<bool>.CreateFailure("Customer not found", 404);

                _unitOfWork.Repository<Customer>().Delete(customerToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0) return ApiResponse<bool>.CreateFailure("Error deleting customer");

                // Invalidate cache after successful deletion
                InvalidateCustomerCache();

                return ApiResponse<bool>.CreateSuccess(true, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error adding Customer", JsonSerializer.Serialize(ex));

                return ApiResponse<bool>.CreateFailure($"Failed to delete customer", 500);
            }

        }
    }
}
