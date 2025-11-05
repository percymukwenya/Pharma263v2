using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class AccountsReceivableStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsReceivableStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetAccountsReceivableStatuses()
        {
            var obj = await _unitOfWork.Repository<AccountsReceivableStatus>().GetAllAsync();

            return [.. obj.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<TypeStatusMethodDetailsResponse> GetAccountsReceivableStatus(int id)
        {
            var obj = await _unitOfWork.Repository<AccountsReceivableStatus>().GetByIdAsync(id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddAccountsReceivableStatus(AddTypeStatusMethodModel request)
        {
            var objToCreate = new AccountsReceivableStatus
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<AccountsReceivableStatus>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return 0; // or throw an exception

            return objToCreate.Id;
        }

        public async Task<bool> UpdateAccountsReceivableStatus(UpdateTypeStatusMethodModel request)
        {
            var existingAccoutReceivableStatus = await _unitOfWork.Repository<AccountsReceivableStatus>().GetByIdAsync(request.Id);

            if (existingAccoutReceivableStatus == null)
                return false; // or throw an exception

            existingAccoutReceivableStatus.Name = request.Name;
            existingAccoutReceivableStatus.Description = request.Description;

            _unitOfWork.Repository<AccountsReceivableStatus>().Update(existingAccoutReceivableStatus);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return false; // or throw an exception

            return true;
        }

        public async Task<bool> DeleteAccountsReceivableStatus(int id)
        {
            var objToDelete = await _unitOfWork.Repository<AccountsReceivableStatus>().GetByIdAsync(id);

            if (objToDelete == null) return false;

            _unitOfWork.Repository<AccountsReceivableStatus>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return false; // or throw an exception

            return true;
        }
    }
}
