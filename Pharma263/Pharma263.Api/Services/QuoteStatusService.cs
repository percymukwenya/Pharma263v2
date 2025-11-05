using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class QuoteStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuoteStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TypeStatusMethodListResponse>> GetQuoteStatuses()
        {
            var obj = await _unitOfWork.Repository<QuoteStatus>().GetAllAsync();

            return [.. obj.Select(x => new TypeStatusMethodListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })];
        }

        public async Task<TypeStatusMethodDetailsResponse> GetQuoteStatus(int id)
        {
            var obj = await _unitOfWork.Repository<QuoteStatus>().FirstOrDefaultAsync(x => x.Id == id);

            return new TypeStatusMethodDetailsResponse
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description
            };
        }

        public async Task<int> AddQuoteStatus(AddTypeStatusMethodModel request)
        {
            var objToCreate = new QuoteStatus
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Repository<QuoteStatus>().AddAsync(objToCreate);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0)
                return 0; // or throw an exception

            return objToCreate.Id;
        }

        public async Task<bool> UpdateQuoteStatus(UpdateTypeStatusMethodModel request)
        {
            var existingQuotationStatus = await _unitOfWork.Repository<QuoteStatus>().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingQuotationStatus == null)
                return false; // or throw an exception

            existingQuotationStatus.Name = request.Name;
            existingQuotationStatus.Description = request.Description;

            _unitOfWork.Repository<QuoteStatus>().Update(existingQuotationStatus);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteQuoteStatus(int id)
        {
            var objToDelete = await _unitOfWork.Repository<QuoteStatus>().FirstOrDefaultAsync(x => x.Id == id);

            if (objToDelete == null)
                return false; // or throw an exception

            _unitOfWork.Repository<QuoteStatus>().Delete(objToDelete);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                return false; // or throw an exception

            return true;
        }
    }
}
