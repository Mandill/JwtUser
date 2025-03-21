using JwtUser.Container;
using JwtUser.Modal;
using JwtUser.Repos.Models;
using JwtUser.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private IUserRoleService _UserRoleService;
        public UserRoleController(IUserRoleService UserRoleService)
        {
            _UserRoleService = UserRoleService;
        }

        [HttpPost("AssignRolePermission")]
        public async Task<IActionResult> AssignRolePermission(List<UserMenuPermissionDto> data)
        {
            var role  = await _UserRoleService.AssignRolePermission(data);
            return Ok(role);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _UserRoleService.GetAllRole();
            return Ok(roles);
        }

        [HttpGet("GetAllMenus")]
        public async Task<IActionResult> GetMenu()
        {
            var menu = await _UserRoleService.GetAllMenu();
            return Ok(menu);
        }

        [HttpGet("GetMenusByRole")]
        public async Task<IActionResult> GetMenusByRole(string role)
        {
            var menu = await _UserRoleService.GetMenusByRole(role);
            return Ok(menu);
        }

        [HttpGet("GetMenuPermissionByRole")]
        public async Task<IActionResult> GetMenuPermissionByRole(string role)
        {
            var menu = await _UserRoleService.GetMenuPermissionByRole(role);
            return Ok(menu);
        }
    }
}
