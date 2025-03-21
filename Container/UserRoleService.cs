using System.Collections.Generic;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using JwtUser.Service;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Container
{
    public class UserRoleService : IUserRoleService
    {
        private readonly LearnDataContext _context;
        public UserRoleService(LearnDataContext context)
        {
            _context = context;
        }
        public async Task<APIResponse<string>> AssignRolePermission(List<UserMenuPermissionDto> data)
        {
            APIResponse<string> response = new APIResponse<string>();
            try
            {
                int count = 0;
                if (data.Count() > 0)
                {
                    using (var dbtransaction = _context.Database.BeginTransaction())
                    {
                        data.ForEach(item =>
                        {
                            var user = _context.TblRolepermissions.FirstOrDefault(x => x.Userrole == item.UserRole
                            && x.Menucode == item.MenuCode);
                            if (user != null)
                            {
                                user.Haveview = item.Permissions.CanView;
                                user.Haveadd = item.Permissions.CanAdd;
                                user.Havedelete = item.Permissions.CanDelete;
                                user.Haveedit = item.Permissions.CanEdit;
                                count++;
                            }
                            else
                            {
                                TblRolepermission TblItem = new TblRolepermission()
                                { 
                                    Menucode = item.MenuCode,
                                    Userrole = item.UserRole,
                                    Haveview = item.Permissions.CanView,
                                    Haveadd = item.Permissions.CanAdd,
                                    Havedelete = item.Permissions.CanDelete,
                                    Haveedit = item.Permissions.CanEdit
                                };

                                _context.TblRolepermissions.Add(TblItem);
                                count++;
                            }
                        });
                        if (count == data.Count)
                        {
                            await this._context.SaveChangesAsync();
                            await dbtransaction.CommitAsync();
                            response.Message = "Role Permission Added Successfully";
                            return response;
                        }
                        else
                        {
                            dbtransaction.Rollback();
                            response.Message = "Transaction rolled back due to mismatch in count";
                            return response;
                        }
                    }
                }
                else
                {
                    response.Message = "Pass atleast 1 permission data";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<APIResponse<List<TblMenu>>> GetAllMenu()
        {
            APIResponse<List<TblMenu>> response = new APIResponse<List<TblMenu>>();
            List<TblMenu> roles = await _context.TblMenus.ToListAsync();

            if (roles.Count > 0)
            {
                response.Result = roles;
                response.Message = "Success";
            }
            else
            {
                response.Message = "No menus found";
                response.Result = new List<TblMenu>();
            }

            return response;
        }
        public async Task<APIResponse<List<TblRole>>> GetAllRole()
        {
            APIResponse<List<TblRole>> response = new APIResponse<List<TblRole>>();
            List<TblRole> roles = await _context.TblRoles.ToListAsync();

            if (roles.Count > 0)
            {
                response.Result = roles;
                response.Message = "Success";
            }
            else
            {
                response.Message = "No menus found";
                response.Result = new List<TblRole>();
            }

            return response;
        }
        public async Task<APIResponse<List<AppMenu>>> GetMenusByRole(string role)
        {
            APIResponse<List<AppMenu>> response = new APIResponse<List<AppMenu>>();
            try
            {
                var result = await (from m in this._context.TblMenus
                              join r in this._context.TblRolepermissions
                              on m.Code equals r.Menucode
                              where r.Userrole == role && r.Haveview
                              select new AppMenu{ code = m.Code, name = m.Name })
                  .Distinct()
                  .ToListAsync();

                if(result.Count() == 0)
                {
                    response.ResponseCode = 400;
                    response.Result = new List<AppMenu>();
                    response.Message = "Menu not found for the role!";
                    return response;
                }

                response.Result = result;
                response.Message = "Success";
                return response;

            }
            catch(Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return response;
            }


        }
        public async Task<APIResponse<List<UserMenuPermissionDto>>> GetMenuPermissionByRole(string role)
        {
            APIResponse<List<UserMenuPermissionDto>> response = new APIResponse<List<UserMenuPermissionDto>>();
            var permissionMenu = await _context.TblRolepermissions.Where(x => x.Userrole == role).ToListAsync();
            var permissionMenuDTO = permissionMenu.Select(x => new UserMenuPermissionDto
            {
                MenuCode = x.Menucode,
                Permissions = new PermissionDto
                {
                    CanAdd = x.Haveadd,
                    CanView = x.Haveview,
                    CanDelete = x.Havedelete,
                    CanEdit = x.Haveedit
                },
                UserRole = x.Userrole
            }).ToList();

            if (permissionMenu.Count() > 0)
            {
                response.Result = permissionMenuDTO;
                response.Message = "Success";
            }
            else
            {
                response.Message = "No permissions found for the role";
                response.Result = new List<UserMenuPermissionDto>();
            }

            return response;
        }
    }
}

