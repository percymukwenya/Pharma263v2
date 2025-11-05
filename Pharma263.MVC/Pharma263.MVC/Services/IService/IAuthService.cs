using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.Models.Response;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponse>> Login(LoginDto model);
        Task<string> Register(RegisterDto model);
        Task<bool> ConfirmEmail(string userId, string code);
        Task ForgotPassword(ForgotPasswordDto model);
        Task<bool> ResetPassword(ResetPasswordDto model);
    }
}
