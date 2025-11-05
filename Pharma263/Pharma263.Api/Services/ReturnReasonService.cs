using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class ReturnReasonService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetReturnReasons()
        {
            var obj = await _unitOfWork.Repository<ReturnReason>().GetAllAsync();

            return [.. obj.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<TypeStatusMethodDetailsResponse> GetReturnReason(int id)
        {
            var obj = await _unitOfWork.Repository<ReturnReason>().FirstOrDefaultAsync(x => x.Id == id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddReturnReason(AddTypeStatusMethodModel request)
        {
            var objToCreate = new ReturnReason
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<ReturnReason>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                return 0; // or throw an exception

            return objToCreate.Id;
        }

        public async Task<bool> UpdateReturnReason(UpdateTypeStatusMethodModel request)
        {
            var existingReturnReason = await _unitOfWork.Repository<ReturnReason>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingReturnReason == null)
                return false; // or throw an exception

            existingReturnReason.Name = request.Name;
            existingReturnReason.Description = request.Description;

            _unitOfWork.Repository<ReturnReason>().Update(existingReturnReason);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return false; // or throw an exception
            }

            return true;
        }

        public async Task<bool> DeleteReturnReason(int id)
        {
            var objToDelete = await _unitOfWork.Repository<ReturnReason>().FirstOrDefaultAsync(x => x.Id == id);

            if (objToDelete == null)
            {
                return false; // or throw an exception
            }

            _unitOfWork.Repository<ReturnReason>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return false; // or throw an exception
            }

            return true;
        }
    }
}
