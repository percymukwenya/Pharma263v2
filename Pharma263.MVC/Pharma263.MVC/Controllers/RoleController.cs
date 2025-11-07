using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.DTOs.User;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    [Authorize] // Require authentication for all role management
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            var roles = await _roleService.GetAllRoles();
            return View(roles.Data);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(AddRoleDto model)
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                var response = await _roleService.CreateRole(model);
                
                if (response.Success)
                {
                    TempData["success"] = "Role created successfully";
                    return RedirectToAction(nameof(Index));
                }
                
                TempData["error"] = response.Message ?? "Failed to create role";
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult EditRole(string roleName)
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            var model = new UpdateRoleDto 
            { 
                OldRoleName = roleName,
                NewRoleName = roleName 
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(UpdateRoleDto model)
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                var response = await _roleService.UpdateRole(model);
                
                if (response.Success)
                {
                    TempData["success"] = "Role updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                
                TempData["error"] = response.Message ?? "Failed to update role";
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RoleDetails(string roleName)
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            var usersInRole = await _roleService.GetUsersInRole(roleName);
            
            ViewBag.RoleName = roleName;
            return View(usersInRole.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (!IsUserAdministrator())
            {
                TempData["error"] = "Access denied. Only Administrators can manage roles.";
                return RedirectToAction("Index", "Dashboard");
            }

            var response = await _roleService.DeleteRole(roleName);
            
            if (response.Success)
            {
                TempData["success"] = "Role deleted successfully";
            }
            else
            {
                TempData["error"] = response.Message ?? "Failed to delete role";
            }
            
            return RedirectToAction(nameof(Index));
        }

        #region Private Methods
        
        private bool IsUserAdministrator()
        {
            var userRoles = HttpContext.Items["UserRoles"] as System.Collections.Generic.List<string>;
            return userRoles != null && userRoles.Contains(UserRoles.Administrator);
        }
        
        #endregion
    }
}
