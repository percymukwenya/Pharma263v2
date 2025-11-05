using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Extensions;
using Pharma263.Application.Contracts.Identity;
using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid user data", ModelState));
            }

            var result = await _userService.AddUser(user);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromQuery] string userId, [FromBody] UpdateUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid user data", ModelState));
            }

            if (userId != user.Id)
            {
                return BadRequest(ApiResponse<bool>.CreateFailure("User ID mismatch", 400));
            }

            var result = await _userService.UpdateUser(user);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("GetUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(string userId)
        {
            var result = await _userService.GetUser(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("GetUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<List<User>>>> GetUsers()
        {
            var result = await _userService.GetUsers();
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("GetUsersPaged")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponse<PaginatedList<User>>>> GetUsersPaged([FromQuery] PagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PaginatedList<User>>(
                    "Invalid request parameters", ModelState));
            }

            var result = await _userService.GetUsersPaged(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddUserToRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid role assignment data", ModelState));
            }

            var result = await _userService.AddUserToRole(role);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("RemoveUserFromRole")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RemoveUserFromRoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid role removal data", ModelState));
            }

            var result = await _userService.RemoveUserFromRole(role);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<bool>.CreateFailure("User ID is required", 400));
            }

            var result = await _userService.DeleteUser(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("GetUserRoles")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRoles([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<List<string>>.CreateFailure("User ID is required", 400));
            }

            var result = await _userService.GetUserRoles(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid password change data", ModelState));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _userService.ChangePassword(userId, model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponse<bool>>> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(ApiResponse<bool>.CreateFailure("Email is required", 400));
            }

            var result = await _userService.ForgotPassword(email);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponse<bool>>> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid password reset data", ModelState));
            }

            var result = await _userService.ResetPassword(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("admin-reset-password")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> AdminResetPassword([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(ApiResponse<bool>.CreateFailure("User ID is required", 400));
            }

            var result = await _userService.ResetUserPassword(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profile = await _userService.GetOwnProfile(userId);
            return Ok(profile);
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<bool>(
                    "Invalid profile data", ModelState));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _userService.UpdateOwnProfile(userId, model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponse<List<User>>>> SearchUsers([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest(ApiResponse<List<User>>.CreateFailure("Search term is required", 400));
            }

            var pagedRequest = new PagedRequest 
            { 
                SearchTerm = searchTerm, 
                Page = 1, 
                PageSize = 50 
            };

            var result = await _userService.GetUsersPaged(pagedRequest);
            if (result.Success)
            {
                return Ok(ApiResponse<List<User>>.CreateSuccess(result.Data.Items, result.Message, result.StatusCode));
            }
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponse<int>>> GetUserCount()
        {
            var result = await _userService.GetUsers();
            if (result.Success)
            {
                return Ok(ApiResponse<int>.CreateSuccess(result.Data.Count, "User count retrieved successfully", 200));
            }
            return StatusCode((int)result.StatusCode, ApiResponse<int>.CreateFailure("Failed to get user count", (int)result.StatusCode));
        }
    }
}
