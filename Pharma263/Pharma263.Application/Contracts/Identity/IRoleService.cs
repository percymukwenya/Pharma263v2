using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Identity
{
    public interface IRoleService
    {
        Task<ApiResponse<List<string>>> GetAllRoles();
        Task<ApiResponse<bool>> CreateRole(AddRoleDto role);
        Task<ApiResponse<bool>> DeleteRole(string roleName);
        Task<ApiResponse<bool>> UpdateRole(UpdateRoleDto role);
        Task<ApiResponse<List<User>>> GetUsersInRole(string roleName);
        Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto model);
        Task<ApiResponse<List<string>>> GetUserRoles(string userId);
        Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto model);
    }
}
