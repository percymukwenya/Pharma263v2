using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserDto>>> GetUsers();
        Task<ApiResponse<PaginatedList<UserDto>>> GetUsersPaged(PagedRequest request);
        Task<DataTableResponse<UserDto>> GetUsersForDataTable(DataTableRequest request);
        Task<ApiResponse<UserDto>> GetUser(string userId);
        Task<ApiResponse<AuthResponse>> GetCurrentUser();
        Task<ApiResponse<bool>> AddUser(AddUserDto user);
        Task<ApiResponse<bool>> UpdateUser(UpdateUserDto user);
        Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto dto);
        Task<ApiResponse<List<string>>> GetUserRoles(string userId);
        Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto dto);
        Task<ApiResponse<bool>> DeleteUser(string userId);

        Task<ApiResponse<bool>> ChangePassword(ChangePasswordDto dto);
        Task<ApiResponse<bool>> ForgotPassword(string email);
        Task<ApiResponse<bool>> ResetPassword(ResetPasswordDto dto);
        Task<ApiResponse<bool>> AdminResetPassword(AdminResetPasswordDto dto);
        Task<ApiResponse<bool>> TriggerPasswordResetEmail(string userId);
        Task<ApiResponse<bool>> UpdateProfile(UserProfileDto profile);
    }
}
