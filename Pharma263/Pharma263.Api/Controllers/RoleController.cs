using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharma263.Application.Contracts.Identity;
using Pharma263.Application.Models.Identity;
using Pharma263.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllRoles()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> CreateRole(AddRoleDto role)
        {
            var result = await _roleService.CreateRole(role);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{roleName}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRole(roleName);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateRole(UpdateRoleDto model)
        {
            var result = await _roleService.UpdateRole(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{roleName}/users")]
        public async Task<ActionResult<List<User>>> GetUsersInRole(string roleName)
        {
            var result = await _roleService.GetUsersInRole(roleName);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("add-user-to-role")]
        public async Task<ActionResult<ApiResponse<bool>>> AddUserToRole(AddUserToRoleDto model)
        {
            var result = await _roleService.AddUserToRole(model);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("remove-user-from-role")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveUserFromRole(RemoveUserFromRoleDto model)
        {
            var result = await _roleService.RemoveUserFromRole(model);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
