using JwtUser.Modal;
using JwtUser.Modal.enums;
using JwtUser.Service;
using Microsoft.AspNetCore.Mvc;

namespace JwtUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration([FromBody] RegisterModel register)
        {
            var response = await _userService.UserRegisterAsync(register);
            return Ok(response);
        }

        [HttpPost("RegistrationConfirmation")]
        public async Task<IActionResult> RegistrationConfirmation([FromBody] RegistrationConfirmationModel register)
        {
            var response = await _userService.ConfirmRegistration(register.UserId, register.Username, register.OtpText);
            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel register)
        {
            var response = await _userService.ResetPassword(register.username, register.oldpassword, register.newpassword);
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel request)
        {
            var response = await _userService.ForgotPassword(request.username);
            return Ok(response);
        }

        [HttpPost("ConfirmForgotPassword")]
        public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordModel request)
        {
            var response = await _userService.ConfirmForgotPassword(request.username, request.otptext, request.newpassword);
            return Ok(response);
        }

        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync(string username, RoleEnum role)
        {
            var response = await _userService.UpdateRole(username, role);
            return Ok(response);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatusAsync(string username, bool status)
        {
            var response = await _userService.UpdateStatus(username, status);
            return Ok(response);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetUserByCodeAsync()
        {

            var response = await _userService.GetAllUserAsync();
            return Ok(response);
        }

        [HttpGet("GetUserByCode")]
        public async Task<IActionResult> GetUserByCodeAsync(string userId)
        {
            var response = await _userService.GetUserByCodeAsync(userId);
            return Ok(response);
        }
    }
}