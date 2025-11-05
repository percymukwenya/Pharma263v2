using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Models;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class SelectionsService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SelectionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectionDto>> GetAccountsPayableStatuses()
        {
            var obj = await _unitOfWork.Repository<AccountsPayableStatus>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = x.Name
            })];
        }

        public async Task<List<SelectionDto>> GetAccountsReceivableStatuses()
        {
            var obj = await _unitOfWork.Repository<AccountsPayableStatus>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = x.Name
            })];
        }

        public async Task<List<SelectionDto>> GetCustomerTypes()
        {
            var obj = await _unitOfWork.Repository<CustomerType>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = x.Name
            })];
        }

        public async Task<List<SelectionDto>> GetCustomers()
        {
            var obj = await _unitOfWork.Repository<Customer>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = x.Name
            })];
        }

        public async Task<List<SelectionDto>> GetSuppliers()
        {
            var obj = await _unitOfWork.Repository<Supplier>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = x.Name
            })];
        }

        public async Task<List<SelectionDto>> GetMedicines()
        {
            var obj = await _unitOfWork.Repository<Medicine>().GetAllAsync();

            obj = obj.OrderBy(x => x.Name);

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }

        public async Task<List<SelectionDto>> GetStocks()
        {
            var obj = await _unitOfWork.Repository<Stock>().GetAllAsync(x => x.Include(x => x.Medicine));

            return [.. obj.Select(x => new SelectionDto
            {
                Id = x.Id,
                Name = $"{x.Medicine.Name} ({x.BatchNo} {x.ExpiryDate.ToString("dd/MM/yyyy")})"
            })];
        }

        public async Task<List<SelectionDto>> GetPaymentMethods()
        {
            var obj = await _unitOfWork.Repository<PaymentMethod>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }

        public async Task<List<SelectionDto>> GetPurchaseStatuses()
        {
            var obj = await _unitOfWork.Repository<PurchaseStatus>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }

        public async Task<List<SelectionDto>> GetSaleStatuses()
        {
            var obj = await _unitOfWork.Repository<SaleStatus>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }

        public async Task<List<SelectionDto>> GetReturnReasons()
        {
            var obj = await _unitOfWork.Repository<ReturnReason>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }

        public async Task<List<SelectionDto>> GetReturnDestinations()
        {
            var obj = await _unitOfWork.Repository<ReturnDestination>().GetAllAsync();

            return [.. obj.Select(x => new SelectionDto { Id = x.Id, Name = x.Name })];
        }
    }
}
