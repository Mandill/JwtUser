using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos.Models;

namespace JwtUser.Service
{
    public interface IUserRoleService
    {
        Task<APIResponse<string>> AssignRolePermission(List<UserMenuPermissionDto> data);
        Task<APIResponse<List<TblRole>>> GetAllRole();
        Task<APIResponse<List<TblMenu>>> GetAllMenu();
        Task<APIResponse<List<AppMenu>>> GetMenusByRole(string role);
        Task<APIResponse<List<UserMenuPermissionDto>>> GetMenuPermissionByRole(string role);
    }
}
