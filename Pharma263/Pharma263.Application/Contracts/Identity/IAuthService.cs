using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> Login(AuthRequest request);
        Task<ApiResponse<string>> Register(RegistrationRequest request);
        Task<ApiResponse<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model);
        Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest model);
        Task<AuthResponse> GetCurrentUser(ApplicationUser user);
        Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string token);
        Task<ApiResponse<bool>> RevokeTokenAsync(string token);
    }
}
