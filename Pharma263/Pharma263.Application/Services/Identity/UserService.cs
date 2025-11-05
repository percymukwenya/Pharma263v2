using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pharma263.Application.Contracts.Email;
using Pharma263.Application.Contracts.Identity;
using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Pharma263.Application.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailSender _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private const string UsersCacheKey = "users_list";
        private const string UserDetailsCacheKeyPrefix = "user_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(10);

        public UserService(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger, 
            IEmailSender emailService,
            IConfiguration configuration,
            IMemoryCache memoryCache)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }
        public string UserId => throw new System.NotImplementedException();

        public async Task<ApiResponse<bool>> AddUser(AddUserDto user)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(user.UserName);

            var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);

            if (userWithSameUserName != null)
            {
                return ApiResponse<bool>.CreateFailure("Username already exists", 400, new List<string> { "Username already exists" });
            }
            else if (userWithSameEmail != null)
            {
                return ApiResponse<bool>.CreateFailure("Email address is in use", 400, new List<string> { "Email address is in use" });
            }
            else
            {
                var identityUser = new ApplicationUser 
                {
                    UserName = user.UserName,
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    PhoneNumber = user.Phone,
                    Email = user.Email
                };

                identityUser.UserName = user.UserName.ToLower();

                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.CreateFailure("User creation failed", 400, [.. result.Errors.Select(e => e.Description)]);
                }

                if (!string.IsNullOrEmpty(user.Role))
                {
                    var roleExists = await _roleManager.FindByNameAsync(user.Role);

                    if (roleExists != null)
                    {
                        var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, user.Role);

                        if (addToRoleResult.Errors.Any())
                        {
                            return ApiResponse<bool>.CreateFailure("Failed to add user to role", 400, ["Failed to add user to role"]);
                        }

                        if (!addToRoleResult.Succeeded)
                        {
                            return ApiResponse<bool>.CreateFailure("Failed to add user to role", 400, new List<string> { "Failed to add user to role" });
                        }
                    }
                    else
                    {
                        return ApiResponse<bool>.CreateFailure($"Role '{user.Role}' does not exist", 400, new List<string> { $"Role '{user.Role}' does not exist" });
                    }
                }

                // Send email confirmation link
                // To do
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                //var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                //var callbackUrl = $"{_configuration["AppUrl"]}/confirm-email?userId={identityUser.Id}&token={encodedToken}";
                //var emailSent = await _emailService.SendEmail(
                //    user.Email,
                //    "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                //);
                //if (!emailSent)
                //{
                //    return ApiResponse<bool>.CreateFailure("Failed to send confirmation email", 500, new List<string> { "Failed to send confirmation email" });
                //}

                InvalidateUserCache();
                return ApiResponse<bool>.CreateSuccess(true, "User created successfully", 200);
            }
        }

        public async Task<ApiResponse<User>> GetUser(string userId)
        {
            try
            {
                var cacheKey = $"{UserDetailsCacheKeyPrefix}{userId}";
                if (_memoryCache.TryGetValue(cacheKey, out User cached))
                {
                    return ApiResponse<User>.CreateSuccess(cached, "User retrieved successfully", 200);
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return ApiResponse<User>.CreateFailure("User not found", 404, ["User not found"]);
                }

                var mappedUser = new User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Phone = user.PhoneNumber,
                    CreatedDate = user.CreatedDate,
                    LastLoginDate = user.LastLoginDate,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed
                };

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles != null || userRoles.Any())
                {
                    mappedUser.Role = userRoles.FirstOrDefault();
                }

                _memoryCache.Set(cacheKey, mappedUser, CacheExpiry);
                return ApiResponse<User>.CreateSuccess(mappedUser, "User retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving user {userId}. {ex.Message}", ex);
                return ApiResponse<User>.CreateFailure($"An error occurred while retrieving user. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<List<User>>> GetUsers()
        {
            try
            {
                if (_memoryCache.TryGetValue(UsersCacheKey, out List<User> cached))
                {
                    return ApiResponse<List<User>>.CreateSuccess(cached, "Users retrieved successfully", 200);
                }

                var users = await _userManager.Users.ToListAsync();

                if (users == null || users.Count == 0)
                {
                    return ApiResponse<List<User>>.CreateFailure("No users found", 404, ["No users found"]);
                }

                var mappedUsers = users.Select(user => new User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Phone = user.PhoneNumber,
                    CreatedDate = user.CreatedDate,
                    LastLoginDate = user.LastLoginDate,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed
                }).ToList();

                foreach (var user in mappedUsers)
                {
                    var localUser = await _userManager.FindByIdAsync(user.Id);
                    var userRoles = await _userManager.GetRolesAsync(localUser);
                    user.Role = userRoles.FirstOrDefault();
                }

                _memoryCache.Set(UsersCacheKey, mappedUsers, CacheExpiry);
                return ApiResponse<List<User>>.CreateSuccess(mappedUsers, "Users retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving users. {ex.Message}", ex);
                return ApiResponse<List<User>>.CreateFailure($"An error occurred while retrieving users. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PaginatedList<User>>> GetUsersPaged(PagedRequest request)
        {
            try
            {
                var query = _userManager.Users.AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    query = query.Where(x => x.FirstName.ToLower().Contains(searchTerm) ||
                                           x.LastName.ToLower().Contains(searchTerm) ||
                                           x.Email.ToLower().Contains(searchTerm) ||
                                           x.UserName.ToLower().Contains(searchTerm));
                }

                query = request.SortBy?.ToLower() switch
                {
                    "firstname" => request.SortDescending ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName),
                    "lastname" => request.SortDescending ? query.OrderByDescending(x => x.LastName) : query.OrderBy(x => x.LastName),
                    "email" => request.SortDescending ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
                    "username" => request.SortDescending ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName),
                    _ => query.OrderBy(x => x.FirstName)
                };

                var totalCount = await query.CountAsync();
                var users = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var mappedUsers = new List<User>();
                foreach (var user in users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    mappedUsers.Add(new User
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Firstname = user.FirstName,
                        Lastname = user.LastName,
                        Phone = user.PhoneNumber,
                        Role = userRoles.FirstOrDefault(),
                        CreatedDate = user.CreatedDate,
                        LastLoginDate = user.LastLoginDate,
                        IsActive = user.IsActive,
                        EmailConfirmed = user.EmailConfirmed
                    });
                }

                var paginatedResult = new PaginatedList<User>(mappedUsers, totalCount, request.Page, request.PageSize);

                return ApiResponse<PaginatedList<User>>.CreateSuccess(paginatedResult, "Paged users retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving paged users. {ex.Message}", ex);
                return ApiResponse<PaginatedList<User>>.CreateFailure($"An error occurred while retrieving paged users. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateUser(UpdateUserDto user)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if (userToUpdate == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", 404, ["User not found"]);
            }
            else
            {

                userToUpdate.Email = user.Email;
                userToUpdate.FirstName = user.Firstname;
                userToUpdate.LastName = user.Lastname;
                userToUpdate.PhoneNumber = user.Phone;

                var updateResult = await _userManager.UpdateAsync(userToUpdate);

                if (!updateResult.Succeeded)
                {
                    return ApiResponse<bool>.CreateFailure("User update failed", 400, updateResult.Errors.Select(e => e.Description).ToList());
                }

                InvalidateUserCache(user.Id);
                return ApiResponse<bool>.CreateSuccess(true, "User updated successfully", 200);
            }
        }

        public async Task<ApiResponse<List<string>>> GetUserRoles(string userId)
        {
            var roleNames = new List<string>();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ApiResponse<List<string>>.CreateFailure("User not found", 404, ["User not found"]);

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                roleNames.Add(role);
            }

            return ApiResponse<List<string>>.CreateSuccess(roleNames, "User roles retrieved successfully", 200);
        }

        public async Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, dto.RoleName);

                if (result.Succeeded)
                {
                    InvalidateUserCache(user.Id);
                    return ApiResponse<bool>.CreateSuccess(true, $"User {user.Email} added to the {dto.RoleName} role", 200);
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure($"Error: Unable to add user {user.Email} to the {dto.RoleName} role", 400, [.. result.Errors.Select(e => e.Description)]);
                }
            }
            else
            {
                return ApiResponse<bool>.CreateFailure("Unable to find user", 404, ["Unable to find user"]);
            }
        }

        public async Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, dto.RoleName);

                if (result.Succeeded)
                {
                    InvalidateUserCache(user.Id);
                    return ApiResponse<bool>.CreateSuccess(true, $"User {user.Email} removed from the {dto.RoleName} role", 200);
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure($"Error: Unable to remove user {user.Email} from the {dto.RoleName} role", 400, [.. result.Errors.Select(e => e.Description)]);
                }
            }
            else
            {
                return ApiResponse<bool>.CreateFailure("Unable to find user", 404, ["Unable to find user"]);
            }
        }

        public async Task<ApiResponse<bool>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", 404, ["User not found"]);
            }

            // Optionally, you can also remove the user from all roles
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                InvalidateUserCache(userId);
                return ApiResponse<bool>.CreateSuccess(true, "User deleted successfully", 200);
            }

            // If we reach here, something went wrong
            _logger.LogError($"Failed to delete user {userId}: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return ApiResponse<bool>.CreateFailure("Failed to delete user", 400, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<ApiResponse<bool>> ResetUserPassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", 404, ["User not found"]);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = GenerateRandomPassword(); // Using our new method
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                // Send the new password to the user via email
                await _emailService.SendEmail(
                    user.Email,
                    "Your Password Has Been Reset",
                    $"Your new password is: {newPassword}. Please change this password after logging in."
                );

                return ApiResponse<bool>.CreateSuccess(true, "Password reset successfully. New password has been sent to the user's email.", 200);
            }

            // If we reach here, something went wrong

            return ApiResponse<bool>.CreateFailure("Failed to reset password", 400, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<ApiResponse<bool>> ChangePassword(string userId, ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", 404, ["User not found"]);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Errors.Any())
            {
                return ApiResponse<bool>.CreateFailure("Error changing password", 400, [.. result.Errors.Select(e => e.Description)]);
            }

            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Password changed successfully", 200);
            }

            return ApiResponse<bool>.CreateFailure("Error changing password", 400, ["Error changing password"]);
        }

        public async Task<ApiResponse<bool>> UpdateOwnProfile(string userId, UpdateProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", 404, ["User not found"]);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Errors.Any())
            {
                return ApiResponse<bool>.CreateFailure("Error updating profile", 400, [.. result.Errors.Select(e => e.Description)]);
            }

            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Profile updated successfully", 200);
            }

            return ApiResponse<bool>.CreateFailure("Error updating profile", 400, ["Error updating profile"]);
        }

        public async Task<User> GetOwnProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Phone = user.PhoneNumber,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed
            };
        }

        public async Task<ApiResponse<bool>> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // We don't want to reveal that the user doesn't exist for security reasons
                return ApiResponse<bool>.CreateSuccess(true, "If a user with this email exists, a password reset link has been sent.", 200);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = $"{_configuration["AppUrl"]}/reset-password?email={email}&token={encodedToken}";

            var emailSent = await _emailService.SendEmail(
                email,
                "Reset your password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            );

            if (emailSent)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Password reset link has been sent to your email.", 200);
            }
            else
            {
                return ApiResponse<bool>.CreateFailure("Failed to send password reset email", 500, ["Failed to send password reset email"]);
            }
        }

        public async Task<ApiResponse<bool>> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // We don't want to reveal that the user doesn't exist for security reasons
                return ApiResponse<bool>.CreateSuccess(true, "If a user with this email exists, the password has been reset.", 200);
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Errors.Any())
            {
                return ApiResponse<bool>.CreateFailure("Error resetting password", 400, [.. result.Errors.Select(e => e.Description)]);
            }

            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Password has been reset successfully", 200);
            }
            else
            {
                return ApiResponse<bool>.CreateFailure("Error resetting password", 400, ["Error resetting password"]);
            }
        }

        private string GenerateRandomPassword(int length = 12)
        {
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            var chars = new List<char>();
            var random = new Random();

            // Ensure at least one character from each category
            chars.Add(uppercaseChars[random.Next(uppercaseChars.Length)]);
            chars.Add(lowercaseChars[random.Next(lowercaseChars.Length)]);
            chars.Add(digitChars[random.Next(digitChars.Length)]);
            chars.Add(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest of the password
            var allChars = uppercaseChars + lowercaseChars + digitChars + specialChars;
            for (int i = chars.Count; i < length; i++)
            {
                chars.Add(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the characters
            for (int i = chars.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            return new string(chars.ToArray());
        }

        public Task<ApiResponse<bool>> AdminResetPassword(AdminResetPasswordDto model)
        {
            throw new NotImplementedException();
        }

        private void InvalidateUserCache()
        {
            _memoryCache.Remove(UsersCacheKey);
        }

        private void InvalidateUserCache(string userId)
        {
            InvalidateUserCache();
            var userCacheKey = $"{UserDetailsCacheKeyPrefix}{userId}";
            _memoryCache.Remove(userCacheKey);
            
            var keys = new List<object>();
            if (_memoryCache is MemoryCache memCache)
            {
                var field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var coherentState = field?.GetValue(memCache);
                var entriesCollection = coherentState?.GetType().GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var entries = (IDictionary)entriesCollection?.GetValue(coherentState);

                if (entries != null)
                {
                    foreach (DictionaryEntry entry in entries)
                    {
                        if (entry.Key.ToString().StartsWith(UserDetailsCacheKeyPrefix))
                        {
                            keys.Add(entry.Key);
                        }
                    }
                }
            }

            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
