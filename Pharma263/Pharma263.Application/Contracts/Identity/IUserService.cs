using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Identity
{
    public interface IUserService
    {
        // Admin calls
        Task<ApiResponse<List<User>>> GetUsers();
        Task<ApiResponse<PaginatedList<User>>> GetUsersPaged(PagedRequest request);
        Task<ApiResponse<User>> GetUser(string userId);
        Task<ApiResponse<bool>> AddUser(AddUserDto user);
        Task<ApiResponse<bool>> UpdateUser(UpdateUserDto user);
        Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto model);
        Task<ApiResponse<List<string>>> GetUserRoles(string userId);
        Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto model);
        Task<ApiResponse<bool>> DeleteUser(string userId);
        Task<ApiResponse<bool>> ResetUserPassword(string userId);
        public string UserId { get; }

        Task<ApiResponse<bool>> ChangePassword(string userId, ChangePasswordDto model);
        Task<ApiResponse<bool>> UpdateOwnProfile(string userId, UpdateProfileDto model);
        Task<User> GetOwnProfile(string userId);

        Task<ApiResponse<bool>> ForgotPassword(string email);
        Task<ApiResponse<bool>> ResetPassword(ResetPasswordDto model);
        Task<ApiResponse<bool>> AdminResetPassword(AdminResetPasswordDto model);
    }
}
