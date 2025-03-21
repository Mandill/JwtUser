using AutoMapper;
using Azure;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Modal.enums;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using JwtUser.Service;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Container
{
    public class UserService : IUserService
    {
        public readonly LearnDataContext _context;
        private readonly IMapper _mapper;

        public UserService(LearnDataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIResponse<string>> UserRegisterAsync(RegisterModel register)
        {
            APIResponse<string> response = new APIResponse<string>();
            try
            {
                //duplicate user
                var duplicateUser = await _context.TblUsers.Where(x => x.Username == register.name).ToListAsync();
                if (duplicateUser.Count > 0)
                {
                    response.ResponseCode = 400;
                    response.Message = $"Username already exists";
                    return response;
                }

                //duplicate email
                var duplicateEmail = await _context.TblUsers.Where(x => x.Email == register.email).ToListAsync();
                if (duplicateEmail.Count > 0)
                {
                    response.ResponseCode = 400;
                    response.Message = $"Email already exists";
                    return response;
                }

                if (register != null)
                {
                    int userid = 0;
                    var user = new TblTempuser
                    {
                        Name = register.name,
                        Code = register.username,
                        Password = register.password,
                        Email = register.email,
                        Phone = register.phone,
                    };

                    await _context.TblTempusers.AddAsync(user);
                    await _context.SaveChangesAsync();
                    string otptext = OTPGenerator();
                    await UpdateOtp(register.username, otptext, "Register");
                    await SendEmail(register.email, otptext);
                    userid = user.Id;
                    response.ResponseCode = 200;
                    response.Message = $"User Registered Successfully {userid}";
                    return response;
                }
                else
                {
                    response.ResponseCode = 400;
                    response.Message = "Invalid Data";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<APIResponse<string>> ConfirmRegistration(int userid, string username, string otptext)
        {
            var apiResponse = new APIResponse<string>();

            if (userid <= 0 || string.IsNullOrEmpty(otptext))
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = "Invalid user ID or OTP";
                return apiResponse;
            }

            var user = await _context.TblOtpManagers
                .Where(x => x.Expiration > DateTime.UtcNow && x.Otptext == otptext)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = "User not found or OTP expired";
                return apiResponse;
            }

            var authUser = await _context.TblTempusers
                .Where(x => x.Id == userid)
                .SingleOrDefaultAsync();

            if (authUser == null)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = "Temporary user not found";
                return apiResponse;
            }

            _context.TblTempusers.Remove(authUser);

            var authenticatedUser = new TblUser
            {
                UserId = user.Id.ToString(),
                Username = authUser.Name,
                Email = authUser.Email,
                Password = authUser.Password,
                Phone = authUser.Phone,
                Isactive = true,
                Role = "user",
            };

            _context.TblUsers.Add(authenticatedUser);
            _context.TblOtpManagers.Remove(user);
            await _context.SaveChangesAsync();
            await UpdatePasswordAsync(authUser.Name, authUser.Password!);
            apiResponse.ResponseCode = 200;
            apiResponse.Message = "User verified with OTP successfully";
            return apiResponse;
        }
        private async Task UpdateOtp(string username, string otptext, string otptype)
        {
            var optvalue = new TblOtpManager
            {
                Username = username,
                Otptext = otptext,
                Otptype = otptype,
                Expiration = DateTime.Now.AddMinutes(30),
                Createddate = DateTime.Now
            };

            await _context.TblOtpManagers.AddAsync(optvalue);
            await _context.SaveChangesAsync();
        }
        private string OTPGenerator()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
        private async Task SendEmail(string email, string otptext)
        {
            var subject = "OTP for Registration";
            var body = $"Your OTP for registration is {otptext}";
        }
        private async Task UpdatePasswordAsync(string username, string password)
        {
            var updatePassword = new TblPwdManger
            {
                Username = username,
                Password = password,
                ModifyDate = DateTime.Now,
            };
            await _context.TblPwdMangers.AddAsync(updatePassword);
            await _context.SaveChangesAsync();
        }
        public async Task<APIResponse<string>> ResetPassword(string username, string oldpassword, string newpassword)
        {
            APIResponse<string> response = new APIResponse<string>();
            try
            {
                TblUser? user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Username == username);
                if (user != null)
                {
                    var validatepwd = await Validatepwdhistory(username, newpassword);
                    if (!string.IsNullOrEmpty(oldpassword) && !string.IsNullOrEmpty(newpassword) && validatepwd)
                    {
                        response.Message = "New password must be different from last 3 of your passwords.";
                        response.ResponseCode = 400;
                        return response;
                    }

                    if (!string.IsNullOrEmpty(oldpassword) && !string.IsNullOrEmpty(newpassword) && user.Password != oldpassword)
                    {
                        response.Message = "Your old password doesn't match";
                        response.ResponseCode = 400;
                        return response;
                    }

                    if (!string.IsNullOrEmpty(oldpassword) && !string.IsNullOrEmpty(newpassword) && user.Password == oldpassword)
                    {
                        user.Password = newpassword;
                        //_context.TblUsers.Update(user);
                        await _context.SaveChangesAsync();
                        await UpdatePasswordAsync(user.Username!, newpassword);
                        response.Message = "Password Updated Successfully";
                        response.ResponseCode = 200;
                        return response;
                    }
                }
                response.Message = "User not found";
                response.ResponseCode = 400;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResponseCode = 400;
                return response;
            }
        }
        private async Task<bool> Validatepwdhistory(string username, string newpassword)
        {
            var pwd = await _context.TblPwdMangers.Where(x => x.Username == username).OrderByDescending(x => x.ModifyDate).Take(3).ToListAsync();
            if (pwd.Count() > 0)
            {
                var oldpwd = pwd.Where(x => x.Password == newpassword);
                if (oldpwd.Any())
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<APIResponse<string>> ForgotPassword(string username)
        {
            APIResponse<string> response = new APIResponse<string>();
            try
            {
                TblUser? user = await _context.TblUsers.SingleOrDefaultAsync(x => x.Username == username);

                if (user != null)
                {
                    var otptext = OTPGenerator();
                    await SendEmail(user.Email, otptext);
                    await UpdateOtp(username, otptext, "Forgot Password");
                    response.Message = $"Please Verify the OTP for {user.UserId}";
                    return response;
                }

                response.ResponseCode = 400;
                response.Message = $"User not found!";
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = $"{ex.Message}";
                return response;
            }
        }
        public async Task<APIResponse<string>> ConfirmForgotPassword(string username, string otptext, string newpassword)
        {
            var apiResponse = new APIResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(otptext))
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "Invalid username or OTP";
                    return apiResponse;
                }

                var userotp = await _context.TblOtpManagers
                    .Where(x => x.Username == username && x.Expiration > DateTime.UtcNow && x.Otptext == otptext)
                    .SingleOrDefaultAsync();

                if (userotp == null)
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "User not found or OTP expired";
                    return apiResponse;
                }

                var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Username == username);

                if (user == null)
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "User not found";
                    return apiResponse;
                }

                bool validatepwdhistory = await Validatepwdhistory(username, newpassword);

                if(validatepwdhistory)
                {
                    apiResponse.Message = "New password must be different from last 3 of your passwords.";
                    apiResponse.ResponseCode = 400;
                    return apiResponse;
                }

                user.Password = newpassword;
                _context.TblUsers.Update(user);
                await _context.SaveChangesAsync();

                await UpdatePasswordAsync(username, newpassword);

                apiResponse.Message = "Password updated successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = ex.Message;
                return apiResponse;
            }
        }
        public async Task<APIResponse<string>> UpdateRole(string username, RoleEnum role)
        {
            var apiResponse = new APIResponse<string>();
            var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Username == username);
            if(user != null)
            {
                user.Role = role.ToString();
                _context.TblUsers.Update(user);
                _context.SaveChanges();
                apiResponse.Message = "Role updated successfully";
                return apiResponse;
            }
            apiResponse.ResponseCode = 400;
            apiResponse.Message = "User not found";
            return apiResponse;
        }
        public async Task<APIResponse<string>> UpdateStatus(string username, bool status)
        {
            var apiResponse = new APIResponse<string>();
            var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Username == username);
            if (user != null)
            {
                user.Isactive = status;
                _context.TblUsers.Update(user);
                _context.SaveChanges();
                apiResponse.Message = "Status updated successfully";
                return apiResponse;
            }
            apiResponse.ResponseCode = 400;
            apiResponse.Message = "User not found";
            return apiResponse;
        }
        public async Task<APIResponse<List<UserDTO>>> GetAllUserAsync()
        {
            APIResponse<List<UserDTO>> response = new APIResponse<List<UserDTO>>();
            try
            {
                var users = await _context.TblUsers.ToListAsync();
                var userDTOs = _mapper.Map<List<UserDTO>>(users);

                if (users.Count() == 0)
                {
                    response.Result = new List<UserDTO>();
                    response.ResponseCode = 400;
                    return response;
                }
                response.Result = userDTOs;
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
        public async Task<APIResponse<UserDTO>> GetUserByCodeAsync(string UserId)
        {
            APIResponse<UserDTO> response = new APIResponse<UserDTO>();
            try
            {
                var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (user == null)
                {
                    response.Result = null;
                    response.Message = "User not found for given userId!";
                    response.ResponseCode = 400;
                    return response;
                }
                var userDTO = _mapper.Map<TblUser, UserDTO>(user);
                response.Result = userDTO;
                response.Message = "Success";
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}