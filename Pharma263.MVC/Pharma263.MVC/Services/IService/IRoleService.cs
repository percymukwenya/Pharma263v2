using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IRoleService
    {
        Task<ApiResponse<List<string>>> GetAllRoles();
        Task<ApiResponse<bool>> CreateRole(AddRoleDto role);
        Task<ApiResponse<List<UserDto>>> GetUsersInRole(string roleName);
        Task<ApiResponse<bool>> UpdateRole(UpdateRoleDto role);
        Task<ApiResponse<bool>> DeleteRole(string roleName);
    }
}
