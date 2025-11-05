using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class ReturnStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetReturnStatuses()
        {
            var obj = await _unitOfWork.Repository<ReturnStatus>().GetAllAsync();

            return obj.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
        }

        public async Task<TypeStatusMethodDetailsResponse> GetReturnStatus(int id)
        {
            var obj = await _unitOfWork.Repository<ReturnStatus>().GetByIdAsync(id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddReturnStatus(AddTypeStatusMethodModel request)
        {
            var objToCreate = new ReturnStatus
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<ReturnStatus>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return 0; // or throw an exception

            return objToCreate.Id;
        }

        public async Task<bool> UpdateReturnStatus(UpdateTypeStatusMethodModel request)
        {
            var existingReturnStatus = await _unitOfWork.Repository<ReturnStatus>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingReturnStatus == null)
                return false; // or throw an exception

            existingReturnStatus.Name = request.Name;
            existingReturnStatus.Description = request.Description;

            _unitOfWork.Repository<ReturnStatus>().Update(existingReturnStatus);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return false; // or throw an exception

            return true;
        }

        public async Task<bool> DeleteReturnStatus(int id)
        {
            var objToDelete = await _unitOfWork.Repository<ReturnStatus>().FirstOrDefaultAsync(x => x.Id == id);

            if (objToDelete == null) return false;

            _unitOfWork.Repository<ReturnStatus>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0) return false;

            return true;
        }
    }
}
