using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class AccountsPayableStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsPayableStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetAccountsPayableStatuses()
        {
            var statuses = await _unitOfWork.Repository<AccountsPayableStatus>().GetAllAsync();

            return [.. statuses.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<TypeStatusMethodDetailsResponse> GetAccountsPayableStatus(int id)
        {
            var obj = await _unitOfWork.Repository<AccountsPayableStatus>().GetByIdAsync(id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddAccountsPayableStatus(AddTypeStatusMethodModel request)
        {
            var objToCreate = new AccountsPayableStatus
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<AccountsPayableStatus>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            return objToCreate.Id;
        }

        public async Task<bool> UpdateAccountsPayableStatus(UpdateTypeStatusMethodModel request)
        {
            var existingAccountsPayableStatus = await _unitOfWork.Repository<AccountsPayableStatus>().GetByIdAsync(request.Id);
            if (existingAccountsPayableStatus == null)
            {
                return false; // or throw an exception
            }

            existingAccountsPayableStatus.Name = request.Name;
            existingAccountsPayableStatus.Description = request.Description;

            _unitOfWork.Repository<AccountsPayableStatus>().Update(existingAccountsPayableStatus);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0) return false; // or throw an exception

            return true;
        }

        public async Task<bool> DeleteAccountsPayableStatus(int id)
        {
            var objToDelete = await _unitOfWork.Repository<AccountsPayableStatus>().GetByIdAsync(id);

            if (objToDelete == null) return false;

            _unitOfWork.Repository<AccountsPayableStatus>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0) return false; // or throw an exception

            return true;
        }
    }
}
