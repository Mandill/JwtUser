using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Modal.enums;
using JwtUser.Repos.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtUser.Service
{
    public interface IUserService
    {
        public Task<APIResponse<string>> UserRegisterAsync(RegisterModel register);
        public Task<APIResponse<string>> ConfirmRegistration(int userid, string username, string otptext);
        public Task<APIResponse<string>> ResetPassword(string username, string oldpassword, string newpassword);
        public Task<APIResponse<string>> ForgotPassword(string username);
        public Task<APIResponse<string>> ConfirmForgotPassword(string username,string otp , string newpassword);
        public Task<APIResponse<string>> UpdateRole(string username, RoleEnum role);
        public Task<APIResponse<string>> UpdateStatus(string username, bool status);
        public Task<APIResponse<List<UserDTO>>> GetAllUserAsync();
        public Task<APIResponse<UserDTO>> GetUserByCodeAsync(string userId);

    }
}
