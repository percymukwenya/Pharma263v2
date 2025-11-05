using Pharma263.Api.Models.CustomerTypes.Request;
using Pharma263.Api.Models.CustomerTypes.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class CustomerTypeService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CustomerTypeListResponse>> GetCustomers()
        {
            var obj = await _unitOfWork.Repository<CustomerType>().GetAllAsync();

            return [.. obj.Select(x => new CustomerTypeListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<CustomerTypeDetailsResponse> GetCustomerType(int id)
        {
            var obj = await _unitOfWork.Repository<CustomerType>().GetByIdAsync(id);

            return new CustomerTypeDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddCustomerType(AddCustomerTypeRequest request)
        {
            var objToCreate = new CustomerType
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<CustomerType>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return 0; // or throw an exception

            return objToCreate.Id;
        }

        public async Task<bool> UpdateCustomerType(UpdateCustomerTypeRequest request)
        {
            var existingCustomerType = await _unitOfWork.Repository<CustomerType>().GetByIdAsync(request.Id);

            if (existingCustomerType == null)
                return false; // or throw an exception

            existingCustomerType.Name = request.Name;
            existingCustomerType.Description = request.Description;

            _unitOfWork.Repository<CustomerType>().Update(existingCustomerType);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return false; // or throw an exception

            return true;
        }

        public async Task<bool> DeleteCustomerType(int id)
        {
            var objToDelete = await _unitOfWork.Repository<CustomerType>().GetByIdAsync(id);

            if (objToDelete == null)
                return false; // or throw an exception

            _unitOfWork.Repository<CustomerType>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return false; // or throw an exception

            return true;
        }
    }
}
