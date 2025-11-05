using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pharma263.Application.Contracts.Identity;
using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pharma263.Application.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> CreateRole(AddRoleDto role)
        {
            if (await _roleManager.RoleExistsAsync(role.RoleName))
            {
                return ApiResponse<bool>.CreateFailure("Role already exists", (int)HttpStatusCode.BadRequest, ["Role already exists"]);
            }

            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                return ApiResponse<bool>.CreateFailure("Role name cannot be empty", (int)HttpStatusCode.BadRequest, ["Role name cannot be empty"]);
            }

            if (role.RoleName.Length > 256)
            {
                return ApiResponse<bool>.CreateFailure("Role name is too long", (int)HttpStatusCode.BadRequest, ["Role name is too long"]);
            }

            if (role.RoleName.Contains(' '))
            {
                return ApiResponse<bool>.CreateFailure("Role name cannot contain spaces", (int)HttpStatusCode.BadRequest, ["Role name cannot contain spaces"]);
            }

            if (role.RoleName.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            {
                return ApiResponse<bool>.CreateFailure("Role name can only contain letters, digits, and underscores", (int)HttpStatusCode.BadRequest, ["Role name can only contain letters, digits, and underscores"]);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Role created successfully", (int)HttpStatusCode.OK);
            }

            return ApiResponse<bool>.CreateFailure("Failed to create role", (int)HttpStatusCode.BadRequest, [.. result.Errors.Select(e => e.Description)]);
        }

        public async Task<ApiResponse<bool>> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return ApiResponse<bool>.CreateFailure("Role not found", (int)HttpStatusCode.NotFound, ["Role not found"]);
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Role deleted successfully", (int)HttpStatusCode.OK);
            }

            if (result.Errors.Any(e => e.Code == "RoleHasUsers"))
            {
                return ApiResponse<bool>.CreateFailure("Role cannot be deleted because it has users", (int)HttpStatusCode.BadRequest, ["Role cannot be deleted because it has users"]);
            }

            return ApiResponse<bool>.CreateFailure("Failed to delete role", (int)HttpStatusCode.BadRequest, [.. result.Errors.Select(e => e.Description)]);
        }

        public async Task<ApiResponse<List<string>>> GetAllRoles()
        {
            var result = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            if (result == null || !result.Any())
            {
                return ApiResponse<List<string>>.CreateFailure("No roles found", (int)HttpStatusCode.NotFound, ["No roles found"]);
            }

            return ApiResponse<List<string>>.CreateSuccess(result, "Roles retrieved successfully", (int)HttpStatusCode.OK);
        }

        public async Task<ApiResponse<List<User>>> GetUsersInRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if (users == null || !users.Any())
            {
                return ApiResponse<List<User>>.CreateFailure("No users found in this role", (int)HttpStatusCode.NotFound, ["No users found in this role"]);
            }

            var mappedUsers = users.Select(user => new User
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Firstname = user.FirstName,
                Lastname = user.LastName
            }).ToList();

            return ApiResponse<List<User>>.CreateSuccess(mappedUsers, "Users retrieved successfully", (int)HttpStatusCode.OK);
        }

        public async Task<ApiResponse<bool>> UpdateRole(UpdateRoleDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.OldRoleName);
            if (role == null)
            {
                return ApiResponse<bool>.CreateFailure("Role not found", (int)HttpStatusCode.NotFound, ["Role not found"]);
            }

            role.Name = model.NewRoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, "Role updated successfully", (int)HttpStatusCode.OK);
            }

            if (result.Errors.Any(e => e.Code == "DuplicateRoleName"))
            {
                return ApiResponse<bool>.CreateFailure("Role name already exists", (int)HttpStatusCode.BadRequest, ["Role name already exists"]);
            }

            if (result.Errors.Any(e => e.Code == "InvalidRoleName"))
            {
                return ApiResponse<bool>.CreateFailure("Invalid role name", (int)HttpStatusCode.BadRequest, ["Invalid role name"]);
            }

            if (result.Errors.Any(e => e.Code == "ConcurrencyFailure"))
            {
                return ApiResponse<bool>.CreateFailure("Concurrency failure", (int)HttpStatusCode.Conflict, ["Concurrency failure"]);
            }

            if (result.Errors.Any(e => e.Code == "RoleNameTooLong"))
            {
                return ApiResponse<bool>.CreateFailure("Role name too long", (int)HttpStatusCode.BadRequest, ["Role name too long"]);
            }

            if (result.Errors.Any(e => e.Code == "InvalidOperation"))
            {
                return ApiResponse<bool>.CreateFailure("Invalid operation", (int)HttpStatusCode.BadRequest, ["Invalid operation"]);
            }

            if (result.Errors.Any(e => e.Code == "InvalidRoleId"))
            {
                return ApiResponse<bool>.CreateFailure("Invalid role ID", (int)HttpStatusCode.BadRequest, ["Invalid role ID"]);
            }

            if (result.Errors.Any(e => e.Code == "InvalidRole"))
            {
                return ApiResponse<bool>.CreateFailure("Invalid role", (int)HttpStatusCode.BadRequest, ["Invalid role"]);
            }

            if (result.Errors.Any(e => e.Code == "RoleNameAlreadyExists"))
            {
                return ApiResponse<bool>.CreateFailure("Role name already exists", (int)HttpStatusCode.BadRequest, ["Role name already exists"]);
            }

            return ApiResponse<bool>.CreateFailure("Failed to update role", (int)HttpStatusCode.BadRequest, [.. result.Errors.Select(e => e.Description)]);
        }

        public async Task<ApiResponse<bool>> AddUserToRole(AddUserToRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", (int)HttpStatusCode.NotFound, ["User not found"]);
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return ApiResponse<bool>.CreateFailure("Role not found", (int)HttpStatusCode.NotFound, ["Role not found"]);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains(model.RoleName))
            {
                return ApiResponse<bool>.CreateFailure("User already in this role", (int)HttpStatusCode.BadRequest, ["User already in this role"]);
            }

            if (userRoles.Count() >= 5)
            {
                return ApiResponse<bool>.CreateFailure("User cannot be in more than 5 roles", (int)HttpStatusCode.BadRequest, ["User cannot be in more than 5 roles"]);
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Errors.Any(e => e.Code == "UserAlreadyInRole"))
            {
                return ApiResponse<bool>.CreateFailure("User already in this role", (int)HttpStatusCode.BadRequest, ["User already in this role"]);
            }

            if (result.Succeeded)
            {
                var userRolesAfter = await _userManager.GetRolesAsync(user);
                if (userRolesAfter.Contains(model.RoleName))
                {
                    return ApiResponse<bool>.CreateSuccess(true, $"User {model.Email} added to role {model.RoleName} successfully", (int)HttpStatusCode.OK);
                }

                return ApiResponse<bool>.CreateFailure("Failed to add user to role", (int)HttpStatusCode.BadRequest, ["Failed to add user to role"]);
            }

            return ApiResponse<bool>.CreateFailure("Failed to add user to role", (int)HttpStatusCode.BadRequest, [.. result.Errors.Select(e => e.Description)]);
        }

        public async Task<ApiResponse<List<string>>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<List<string>>.CreateFailure("User not found", (int)HttpStatusCode.NotFound, ["User not found"]);
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null || !roles.Any())
            {
                return ApiResponse<List<string>>.CreateFailure("No roles found for this user", (int)HttpStatusCode.NotFound, ["No roles found for this user"]);
            }

            return ApiResponse<List<string>>.CreateSuccess(roles.ToList(), "Roles retrieved successfully", (int)HttpStatusCode.OK);
        }

        public async Task<ApiResponse<bool>> RemoveUserFromRole(RemoveUserFromRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponse<bool>.CreateFailure("User not found", (int)HttpStatusCode.NotFound, ["User not found"]);
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return ApiResponse<bool>.CreateFailure("Role not found", (int)HttpStatusCode.NotFound, ["Role not found"]);
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return ApiResponse<bool>.CreateSuccess(true, $"User {model.Email} removed from role {model.RoleName} successfully", (int)HttpStatusCode.OK);
            }

            return ApiResponse<bool>.CreateFailure("Failed to remove user from role", (int)HttpStatusCode.BadRequest, ["Failed to remove user from role"]);

        }
    }
}
