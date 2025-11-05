using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pharma263.Application.Configurations;
using Pharma263.Application.Contracts.Email;
using Pharma263.Application.Contracts.Identity;
using Pharma263.Application.Models.Email;
using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pharma263.Application.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly JWTOptions _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager,
            IOptions<JWTOptions> jwtSettings,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<ApiResponse<AuthResponse>> Login(AuthRequest request)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == request.UserName.ToLower());

            if (user == null)
            {
                return ApiResponse<AuthResponse>.CreateFailure("Username or password is incorrect", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    return ApiResponse<AuthResponse>.CreateFailure("Username or password is incorrect", (int)HttpStatusCode.BadRequest);
                }
                else
                {
                    JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                    var data = new AuthResponse
                    {
                        Id = user.Id,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        Email = user.Email,
                        UserName = user.UserName
                    };

                    return ApiResponse<AuthResponse>.CreateSuccess(data, "Login successful", (int)HttpStatusCode.OK);
                }
            }
        }

        public async Task<ApiResponse<string>> Register(RegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return ApiResponse<string>.CreateSuccess($"Account Confirmed for {user.Email}.", "Email confirmed successfully", (int)HttpStatusCode.OK);
            }
            else
            {
                return ApiResponse<string>.CreateFailure($"Error occured while confirming email.", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task ForgotPassword(ForgotPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "auth/resetpassword";
            var _enpointUri = new Uri(string.Concat($"https://pharma263.com/", route));
            var resetPasswordUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "code", code);
            var emailRequest = new EmailMessage
            {
                Body = $"Please reset your password by <a href='{resetPasswordUri}'>clicking here</a>.",
                To = model.Email,
                Subject = "Reset Password",
            };

            await _emailSender.SendEmail(emailRequest.To, emailRequest.Subject, emailRequest.Body);
        }

        public async Task<AuthResponse> GetCurrentUser(ApplicationUser user)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            var userObj = new AuthResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return userObj;
        }

        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null)
            {
                return ApiResponse<string>.CreateFailure($"No Accounts Registered with {model.Email}.", (int)HttpStatusCode.NotFound);
            }

            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return ApiResponse<string>.CreateSuccess($"Password Reset Successful for {model.Email}.", "Password reset successfully", (int)HttpStatusCode.OK);
            }
            else
            {
                return ApiResponse<string>.CreateFailure($"Error occured while reseting the password.", (int)HttpStatusCode.BadRequest);
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
               signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
