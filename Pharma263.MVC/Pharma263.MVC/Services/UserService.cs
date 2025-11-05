using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Models.Response;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{

    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<UserDto>>> GetUsers()
        {
            return await _apiService.GetApiResponseAsync<List<UserDto>>("/api/User/GetUsers");
        }

        public async Task<ApiResponse<UserDto>> GetUser(string userId)
        {
            return await _apiService.GetApiResponseAsync<UserDto>($"/api/User/GetUser?userId={userId}");
        }

        public async Task<ApiResponse<AuthResponse>> GetCurrentUser()
        {
            return await _apiService.GetApiResponseAsync<AuthResponse>($"/api/Account");
        }

        public async Task<ApiResponse<bool>> AddUser(AddUserDto user)
        {
            var request = new CreateUserRequest
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                UserName = user.UserName,
                Role = user.Role
            };

            return await _apiService.PostApiResponseAsync<bool>("/api/User/AddUser", request);
        }

        public async Task<ApiResponse<bool>> UpdateUser(UpdateUserDto user)
        {
            var request = new UpdateUserRequest
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Phone = user.Phone,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role
            };

            return await _apiService.PutApiResponseAsync<bool>($"/api/User/UpdateUser?userId={user.Id}", request);
        }

        public async Task<ApiResponse<List<string>>> GetUserRoles(string userId)
        {
            return await _apiService.GetApiResponseAsync<List<string>>($"/api/User/GetUserRoles?userId={userId}");
        }

        public async Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto dto)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/User/AddUserToRole", dto);
        }

        public async Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto dto)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/User/RemoveUserFromRole", dto);
        }

        public async Task<ApiResponse<bool>> ChangePassword(ChangePasswordDto dto)
        {
            return await _apiService.PostApiResponseAsync<bool>($"/api/User/change-password", dto);
        }

        public async Task<ApiResponse<bool>> ForgotPassword(string email)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/User/forgot-password", new { Email = email });
        }

        public async Task<ApiResponse<bool>> ResetPassword(ResetPasswordDto dto)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/User/reset-password", dto);
        }

        public async Task<ApiResponse<bool>> DeleteUser(string userId)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/User/DeleteUser?userId={userId}");
        }

        public async Task<ApiResponse<bool>> AdminResetPassword(AdminResetPasswordDto dto)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/User/admin-reset-password", dto);
        }

        public async Task<ApiResponse<bool>> TriggerPasswordResetEmail(string userId)
        {
            return await _apiService.PostApiResponseAsync<bool>($"/api/User/trigger-password-reset-email", new { UserId = userId });
        }

        public async Task<ApiResponse<bool>> UpdateProfile(UserProfileDto profile)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/User/update-profile", profile);
        }

        public async Task<ApiResponse<PaginatedList<UserDto>>> GetUsersPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={request.SearchTerm}&SortBy={request.SortBy}&SortDescending={request.SortDescending}";
            return await _apiService.GetApiResponseAsync<PaginatedList<UserDto>>($"/api/User/GetUsersPaged?{queryString}");
        }

        public async Task<DataTableResponse<UserDto>> GetUsersForDataTable(DataTableRequest request)
        {
            try
            {
                var pagedRequest = new PagedRequest
                {
                    Page = (request.Start / request.Length) + 1,
                    PageSize = request.Length,
                    SearchTerm = request.Search?.Value,
                    SortDescending = request.Order?.FirstOrDefault()?.Dir == "desc",
                    SortBy = GetSortColumn(request)
                };

                var apiResponse = await GetUsersPaged(pagedRequest);
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<UserDto>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<UserDto>
                    {
                        Draw = request.Draw,
                        RecordsTotal = 0,
                        RecordsFiltered = 0,
                        Error = apiResponse.Message
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new DataTableResponse<UserDto>
                {
                    Draw = request.Draw,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Error = ex.Message
                };
            }
        }

        private string GetSortColumn(DataTableRequest request)
        {
            if (request.Order == null || !request.Order.Any()) 
                return null;

            var orderColumn = request.Order.First();
            
            return orderColumn.Column switch
            {
                0 => "firstname",
                1 => "lastname", 
                2 => "email",
                3 => "userName",
                4 => "role",
                _ => null
            };
        }
    }
}
