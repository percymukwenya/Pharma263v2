using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models.Quotation.Request;
using Pharma263.Api.Models.Quotation.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Models;
using Pharma263.Application.Services.Printing;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class QuotationService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IAppLogger<QuotationService> _logger;

        public QuotationService(IUnitOfWork unitOfWork, IQuotationRepository quotationRepository,
            IStockRepository stockRepository, IAppLogger<QuotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _quotationRepository = quotationRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<List<QuotationListResponse>>> GetQuotations()
        {
            try
            {
                var quotations = await _unitOfWork.Repository<Quotation>().GetAllAsync(query => query.Include(c => c.Customer).Include(c => c.Items).Include(x => x.QuoteStatus));

                var quotationList = quotations.Select(x => new QuotationListResponse
                {
                    Id = x.Id,
                    QuotationDate = x.QuotationDate,
                    CustomerName = x.Customer.Name,
                    Notes = x.Notes,
                    Total = (double)x.Total,
                    Discount = (double)x.Discount,
                    GrandTotal = (double)x.GrandTotal,
                    QuotationStatus = x.QuoteStatus.Name,
                    CustomerAddress = x.Customer.DeliveryAddress,
                    CustomerPhone = x.Customer.Phone
                }).ToList();

                quotationList = quotationList.OrderByDescending(x => x.QuotationDate).ToList();

                return ApiResponse<List<QuotationListResponse>>.CreateSuccess(quotationList);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quotations. {ex.Message}", ex);

                return ApiResponse<List<QuotationListResponse>>.CreateFailure($"An error occurred while retrieving quotations. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<QuotationDetailsResponse>> GetQuotation(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Repository<Quotation>().GetByIdWithIncludesAsync(id, query => query.Include(c => c.Customer).Include(c => c.Items).Include(c => c.QuoteStatus));

                // convert data object to DTO object
                var quotationDetails = new QuotationDetailsResponse
                {
                    Id = quotation.Id,
                    QuotationDate = quotation.QuotationDate,
                    CustomerName = quotation.Customer.Name,
                    Notes = quotation.Notes,
                    Total = (double)quotation.Total,
                    Discount = (double)quotation.Discount,
                    GrandTotal = (double)quotation.GrandTotal,
                    QuotationStatusId = quotation.QuoteStatusId,
                    QuotationStatus = quotation.QuoteStatus.Name,
                    CustomerAddress = quotation.Customer.DeliveryAddress,
                    CustomerPhone = quotation.Customer.Phone,
                    CustomerId = quotation.CustomerId,
                    Items = quotation.Items.Select(item => new GetQuotationItemsResponse
                    {
                        Id = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = item.Amount,
                        StockId = item.StockId,
                        MedicineName = string.Empty
                    }).ToList()
                };

                foreach (var item in quotationDetails.Items)
                {
                    var stockItem = await _unitOfWork.Repository<Stock>().GetByIdWithIncludesAsync(item.StockId, query => query.Include(c => c.Medicine));

                    item.MedicineName = stockItem.Medicine.Name;
                    item.MedicineId = stockItem.MedicineId;
                    item.StockId = stockItem.Id;
                }

                return ApiResponse<QuotationDetailsResponse>.CreateSuccess(quotationDetails);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quotation details. {ex.Message}", ex);

                return ApiResponse<QuotationDetailsResponse>.CreateFailure($"An error occurred while retrieving quotation details. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetQuotationDoc(int id)
        {
            var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();
            var storeInfo = store.FirstOrDefault();

            var quotation = await _unitOfWork.Repository<Quotation>().GetByIdWithIncludesAsync(id, query => query.Include(c => c.Customer).Include(c => c.QuoteStatus).Include(c => c.Items).ThenInclude(x => x.Stock));

            var quotationDto = new QuotationDto
            {
                Id = quotation.Id,
                QuotationDate = quotation.QuotationDate,
                CustomerName = quotation.Customer.Name,
                Notes = quotation.Notes,
                Total = (double)quotation.Total,
                Discount = (double)quotation.Discount,
                GrandTotal = (double)quotation.GrandTotal,
                QuotationStatus = quotation.QuoteStatus.Name,
                CustomerAddress = quotation.Customer.DeliveryAddress,
                CustomerPhone = quotation.Customer.Phone,
                Items = [.. quotation.Items.Select(x => new GetQuotationItemDto
                {
                     Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Discount = x.Discount,
                    Amount = x.Amount,
                    StockId = x.StockId,
                    MedicineName = string.Empty,
                    QuotationId = x.QuotationId
                })]
            };

            foreach (var item in quotationDto.Items)
            {
                var stockItem = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.Id == item.StockId, q => q.Include(x => x.Medicine));

                if (stockItem != null)
                {
                    item.MedicineId = stockItem.MedicineId;
                    item.MedicineName = stockItem.Medicine.Name;
                    item.BatchNo = stockItem.BatchNo;
                    item.ExpiryDate = stockItem.ExpiryDate;
                }
            }

            if (quotation != null)
            {
                var qoute = new QuotationReportViewModel
                {
                    Company = new StoreSettingsDetailsDto
                    {
                        Id = storeInfo.Id,
                        Logo = storeInfo.Logo,
                        StoreName = storeInfo.StoreName,
                        Email = storeInfo.Email,
                        Phone = storeInfo.Phone,
                        Currency = storeInfo.Currency,
                        Address = storeInfo.Address,
                        MCAZLicence = storeInfo.MCAZLicence,
                        VATNumber = storeInfo.VATNumber,
                        BankingDetails = storeInfo.BankingDetails,
                        ReturnsPolicy = storeInfo.ReturnsPolicy
                    },
                    Quotation = quotationDto
                };
                QuotationInvoiceReport salesReport = new QuotationInvoiceReport();
                byte[] bytes = salesReport.CreateReport(qoute);
                return bytes;
            }
            return null;
        }

        public async Task<ApiResponse<bool>> AddQuotation(AddQuotationRequest request)
        {
            try
            {
                var quoteToCreate = new Quotation
                {
                    CustomerId = request.CustomerId,
                    QuotationDate = request.QuotationDate,
                    Notes = request.Notes,
                    Total = request.Total,
                    Discount = request.Discount,
                    GrandTotal = request.GrandTotal,
                    QuoteExpiryDate = DateTime.UtcNow.AddHours(2).AddDays(2),
                    QuoteStatusId = request.QuoteStatusId,
                };

                quoteToCreate.Items = new List<QuotationItems>();

                quoteToCreate.QuoteStatusId = 5;

                var errors = new List<string>();

                foreach (var item in request.Items)
                {
                    // Calculate the net amount: (Price * Quantity) - Discount.
                    decimal netAmount = (item.Price * item.Quantity) - item.Discount;
                    if (netAmount < 0)
                    {
                        errors.Add($"The discount on an item cannot exceed its line total: {netAmount}.");
                    }

                    // Create a new quotation item and add it to the Quotation entity
                    var quotationItem = new QuotationItems
                    {
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = netAmount,
                        StockId = item.StockId,
                    };

                    quoteToCreate.Items.Add(quotationItem);
                }

                if (errors.Count != 0)
                {
                    return ApiResponse<bool>.CreateFailure("Error adding quotation items", 400, errors);
                }

                await _unitOfWork.Repository<Quotation>().AddAsync(quoteToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                {
                    return ApiResponse<bool>.CreateFailure("Error adding quotation", 400, errors);
                }

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while adding quotation. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<bool>> UpdateQuotation(UpdateQuotationRequest request)
        {
            try
            {
                var quoteToUpdate = await _unitOfWork.Repository<Quotation>()
                        .GetByIdWithIncludesAsync(request.Id, query => query.Include(q => q.Items));

                if (quoteToUpdate == null)
                    return ApiResponse<bool>.CreateFailure("Quotation not found.", 404);

                // Optionally remove existing items.
                if (quoteToUpdate.Items.Any())
                {
                    foreach (var existingItem in quoteToUpdate.Items.ToList())
                    {
                        _unitOfWork.Repository<QuotationItems>().Delete(existingItem);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Update the header details from the request.
                quoteToUpdate.CustomerId = request.CustomerId;
                quoteToUpdate.QuotationDate = request.QuotationDate;
                quoteToUpdate.Total = request.Total;
                quoteToUpdate.Discount = request.Discount;
                quoteToUpdate.GrandTotal = request.GrandTotal;
                quoteToUpdate.Notes = request.Notes;

                var errors = new List<string>();
                foreach (var item in request.Items)
                {
                    decimal netAmount = (item.Price * item.Quantity) - item.Discount;
                    if (netAmount < 0)
                        errors.Add($"The discount {item.Discount} on an item cannot exceed its line total.");

                    quoteToUpdate.Items.Add(new QuotationItems
                    {
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = netAmount,
                        StockId = item.StockId
                    });
                }

                _unitOfWork.Repository<Quotation>().Update(quoteToUpdate);
                if (errors.Count != 0)
                {
                    return ApiResponse<bool>.CreateFailure("Error updating quotation items", 400, errors);
                }

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error updating quotation.", 400);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating quotation. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<bool>> FinalizeQuotationToSale(int quotationId)
        {
            try
            {
                // Retrieve the quotation including its items.
                var quotation = await _unitOfWork.Repository<Quotation>()
                                        .GetByIdWithIncludesAsync(quotationId, query => query.Include(q => q.Items));
                if (quotation == null)
                    throw new Exception("Quotation not found.");

                // Create a new Sale entity and map details from the quotation.
                var sale = new Sales
                {
                    CustomerId = quotation.CustomerId,
                    SalesDate = DateTime.Now,
                    Notes = quotation.Notes,
                    Total = quotation.Total,
                    Discount = quotation.Discount,
                    GrandTotal = quotation.GrandTotal,
                    SaleStatusId = 1, // e.g., Pending or Finalized
                    Items = new List<SalesItems>()
                };

                // Process each quotation item
                foreach (var quoteItem in quotation.Items)
                {
                    decimal netAmount = (quoteItem.Price * quoteItem.Quantity) - quoteItem.Discount;
                    if (netAmount < 0)
                        throw new Exception("Invalid discount calculation.");

                    var saleItem = new SalesItems
                    {
                        StockId = quoteItem.StockId,
                        Price = quoteItem.Price,
                        Quantity = quoteItem.Quantity,
                        Discount = quoteItem.Discount,
                        Amount = netAmount
                    };

                    sale.Items.Add(saleItem);

                    // Update stock: deduct sold quantity
                    var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.Id == quoteItem.StockId);
                    if (stock != null)
                    {
                        await _stockRepository.SubQuantity(quoteItem.Quantity, stock.Id);
                    }
                    else
                    {
                        throw new Exception($"Stock not found for StockId {quoteItem.StockId}");
                    }
                }

                // Save the new sale
                await _unitOfWork.Repository<Sales>().AddAsync(sale);
                await _unitOfWork.SaveChangesAsync();

                // Optionally update the quotation to mark it as finalized
                //quotation.QuoteStatusId = /* A status value representing 'Converted' */;
                _unitOfWork.Repository<Quotation>().Update(quotation);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while finalizing quotation to sale. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while finalizing quotation to sale. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteQuotation(int id)
        {
            try
            {
                var quoteToDelete = await _unitOfWork.Repository<Quotation>().FirstOrDefaultAsync(x => x.Id == id);

                if (quoteToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Quotation not found.", 404);

                var items = await _unitOfWork.Repository<QuotationItems>().FindAsync(x => x.QuotationId == id);

                _unitOfWork.Repository<Quotation>().Delete(quoteToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error deleting quotation.", 400);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting quotation. {ex.Message}", 500);
            }
        }
    }
}
