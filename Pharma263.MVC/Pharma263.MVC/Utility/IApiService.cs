using Pharma263.Integration.Api.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Utility
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<List<T>> GetAllAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, T item);
        Task<T> PutAsync<T>(string endpoint, T item);
        Task<bool> DeleteAsync(string endpoint);

        Task<ApiResponse<T>> GetApiResponseAsync<T>(string endpoint);
        Task<ApiResponse<T>> PostApiResponseAsync<T>(string endpoint, object data);
        Task<ApiResponse<T>> PutApiResponseAsync<T>(string endpoint, object data);
        Task<ApiResponse<bool>> DeleteApiResponseAsync(string endpoint);
    }
}
