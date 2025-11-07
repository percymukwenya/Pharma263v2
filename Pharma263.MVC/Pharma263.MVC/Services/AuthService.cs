using AutoMapper;
using Humanizer;
using Pharma263.Integration.Api;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.Models.Response;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPharmaApiService _pharmaApiService;
        private readonly IMapper _mapper;
        private readonly IApiService _apiService;

        public AuthService(IPharmaApiService pharmaApiService, IMapper mapper, IApiService apiService)
        {
            _pharmaApiService = pharmaApiService;
            _mapper = mapper;
            _apiService = apiService;
        }

        public async Task<ApiResponse<AuthResponse>> Login(LoginDto model)
        {
            var response = await _apiService.PostApiResponseAsync<AuthResponse>("/api/Account/login", model);

            return response;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            var response = await _pharmaApiService.ConfirmEmail(userId, code);

            if (response != null && response.IsSuccess)
            {
                return true;
            }

            return false;
        }

        public async Task ForgotPassword(ForgotPasswordDto model)
        {
            var obj = _mapper.Map<ForgotPasswordRequest>(model);

            await _pharmaApiService.ForgotPassword(obj);
        }        

        public Task<string> Register(RegisterDto model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ResetPassword(ResetPasswordDto model)
        {
            var obj = _mapper.Map<ResetPasswordRequest>(model);

            var response = await _pharmaApiService.ResetPassword(obj);

            if (response != null && response.IsSuccess)
            {
                return true;
            }

            return false;
        }
    }
}
