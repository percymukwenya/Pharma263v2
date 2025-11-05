using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class ReturnDestinationService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<ReturnDestinationService> _logger;

        public ReturnDestinationService(IUnitOfWork unitOfWork, IAppLogger<ReturnDestinationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetReturnDestinations()
        {
            var obj = await _unitOfWork.Repository<ReturnDestination>().GetAllAsync();

            return [.. obj.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<TypeStatusMethodDetailsResponse> GetReturnDestination(int id)
        {
            var obj = await _unitOfWork.Repository<ReturnDestination>().FirstOrDefaultAsync(x => x.Id == id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddReturnDestination(AddTypeStatusMethodModel request)
        {
            var objToCreate = new ReturnDestination
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<ReturnDestination>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            return objToCreate.Id;
        }

        public async Task<bool> UpdateReturnDestination(UpdateTypeStatusMethodModel request)
        {
            var existingReturnDestination = await _unitOfWork.Repository<ReturnDestination>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingReturnDestination == null)
            {
                return false; // or throw an exception
            }

            existingReturnDestination.Name = request.Name;
            existingReturnDestination.Description = request.Description;

            _unitOfWork.Repository<ReturnDestination>().Update(existingReturnDestination);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return false; // or throw an exception
            }

            return true;
        }

        public async Task<bool> DeleteReturnDestination(int id)
        {
            var objToDelete = await _unitOfWork.Repository<ReturnDestination>().FirstOrDefaultAsync(x => x.Id == id);

            if (objToDelete == null)
            {
                return false; // or throw an exception
            }

            _unitOfWork.Repository<ReturnDestination>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                return false; // or throw an exception
            }

            return true;
        }
    }
}
