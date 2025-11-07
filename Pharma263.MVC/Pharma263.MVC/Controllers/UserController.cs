using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Auth;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Services.IService;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddUser()
        {
            var roles = await _roleService.GetAllRoles();
            var model = new AddUserDto
            {
                Roles = roles.Data.Select(r => new SelectListItem { Value = r, Text = r }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(AddUserDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.AddUser(model);

                if (response.Success)
                {
                    TempData["success"] = "User created successfully";
                    return RedirectToAction(nameof(UserList));
                }
                TempData["error"] = response.Message;
            }
            var roles = await _roleService.GetAllRoles();
            model.Roles = roles.Data.Select(r => new SelectListItem { Value = r, Text = r }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditUser(string userId)
        {
            var roles = await _roleService.GetAllRoles();
            if (roles == null || roles.Data == null)
            {
                TempData["error"] = "Failed to load roles";
                return RedirectToAction(nameof(UserList));
            }

            var user = await _userService.GetUser(userId);

            if (user == null)
            {
                TempData["error"] = "User not found";
                return RedirectToAction(nameof(UserList));
            }

            var model = new UpdateUserDto
            {
                Id = user.Data.Id,
                Email = user.Data.Email,
                Firstname = user.Data.Firstname,
                Lastname = user.Data.Lastname,
                Phone = user.Data.Phone,
                UserName = user.Data.UserName,
                Role = user.Data.Role,
                Roles = roles.Data.Select(r => new SelectListItem { Value = r, Text = r }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UpdateUserDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.UpdateUser(model);
                if (response.Success)
                {
                    TempData["success"] = "User updated successfully";
                    return RedirectToAction(nameof(UserList));
                }
                TempData["error"] = "Failed to update user";
            }
            var roles = await _roleService.GetAllRoles();
            model.Roles = roles.Data.Select(r => new SelectListItem { Value = r, Text = r }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Details(string userId)
        {
            var user = await _userService.GetUser(userId);

            return View(user.Data);
        }

        public async Task<ActionResult> DeleteUser(string userId)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.DeleteUser(userId);
                if (response.Success)
                {
                    TempData["success"] = "User deleted successfully";
                    return RedirectToAction(nameof(UserList));
                }
                TempData["error"] = "Failed to delete user";
            }
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeOwnPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeOwnPassword(ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.ChangePassword(model);

                if (response.Success)
                {
                    TempData["success"] = "Your password has been changed successfully";
                    return RedirectToAction("Index", "Home");
                }

                TempData["error"] = "Failed to change password";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var response = await _userService.ForgotPassword(email);
            if (response.Success)
            {
                TempData["success"] = "Password reset instructions sent to your email";
                return RedirectToAction("Login", "Auth");
            }
            TempData["error"] = "Failed to process forgot password request";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordDto { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.ResetPassword(model);
                if (response.Success)
                {
                    TempData["success"] = "Password reset successfully";
                    return RedirectToAction(nameof(UserList));
                }
                TempData["error"] = "Failed to reset password";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ResetUserPassword(string userId)
        {
            var model = new AdminResetPasswordDto { UserId = userId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetUserPassword(AdminResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.AdminResetPassword(model);
                if (response.Success)
                {
                    TempData["success"] = "Password reset successfully";
                    return RedirectToAction("Details", new { userId = model.UserId });
                }
                TempData["error"] = response.Message ?? "Failed to reset password";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TriggerPasswordResetEmail(string userId)
        {
            var response = await _userService.TriggerPasswordResetEmail(userId);
            if (response.Success)
            {
                TempData["success"] = "Password reset email sent successfully";
            }
            else
            {
                TempData["error"] = response.Message ?? "Failed to send password reset email";
            }
            return RedirectToAction("Details", new { userId });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var userId = User.FindFirstValue("uid");
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
            var user = await _userService.GetUser(userId);

            if (user == null || !user.Success)
            {
                TempData["error"] = "Failed to load your profile";
                return RedirectToAction("Index", "Dashboard");
            }

            var model = new UserProfileDto
            {
                Id = user.Data.Id,
                Email = user.Data.Email,
                Firstname = user.Data.Firstname,
                Lastname = user.Data.Lastname,
                Phone = user.Data.Phone,
                UserName = user.Data.UserName
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(UserProfileDto model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue("uid");
                if (userId != model.Id)
                {
                    TempData["error"] = "Invalid user data";
                    return RedirectToAction("Index", "Home");
                }

                var response = await _userService.UpdateProfile(model);
                if (response.Success)
                {
                    TempData["success"] = "Profile updated successfully";
                    return RedirectToAction("Profile");
                }
                TempData["error"] = response.Message ?? "Failed to update profile";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetUsersDataTable([FromBody] DataTableRequest request)
        {
            var response = await _userService.GetUsersForDataTable(request);
            return Json(response);
        }
    }
}
