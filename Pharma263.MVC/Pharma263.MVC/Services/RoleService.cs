using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class RoleService : IRoleService
    {
        private readonly IApiService _apiService;

        public RoleService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<string>>> GetAllRoles()
        {
            return await _apiService.GetApiResponseAsync<List<string>>("/api/Role");
        }

        public async Task<ApiResponse<bool>> CreateRole(AddRoleDto role)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/Role", role);
        }

        public async Task<ApiResponse<bool>> UpdateRole(UpdateRoleDto role)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/Role", role);
        }

        public async Task<ApiResponse<List<UserDto>>> GetUsersInRole(string roleName)
        {
            return await _apiService.GetApiResponseAsync<List<UserDto>>($"/api/Role/{roleName}/users");
        }

        public async Task<ApiResponse<bool>> DeleteRole(string roleName)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/Role/{roleName}");
        }
    }
}
