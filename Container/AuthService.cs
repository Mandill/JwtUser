using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using JwtUser.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtUser.Container
{
    public class AuthService : IAuthService
    {
        private readonly LearnDataContext _context;
        private readonly ServiceConfiguration _appsettings;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, LearnDataContext context, IOptions<ServiceConfiguration> settings)
        {
            _context = context;
            _mapper = mapper;
            _appsettings = settings.Value;
        }

        public async Task<APIResponse<TokenModel>> LoginAsync(LoginModel login)
        {
            APIResponse<TokenModel> apiResponse = new APIResponse<TokenModel>();

            TblUser? user = _context.TblUsers.FirstOrDefault(x => x.Username == login.UserName && x.Password == login.Password);

            if (user == null)
            {
                apiResponse.ResponseCode = 404;
                apiResponse.Message = "Invalid username or password";
                return apiResponse;
            }

            UserModel userModel = _mapper.Map<TblUser, UserModel>(user!);
            AuthResult authenticationResult = await AuthenticateAsync(userModel);

            if (authenticationResult != null && authenticationResult.Success)
            {
                apiResponse.Result = new TokenModel() { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken ,Role = user.Role! ,Username = user.Username! };
                apiResponse.Message = "Success";
            }
            return apiResponse;
        }

        public async Task<AuthResult> AuthenticateAsync(UserModel user)
        {
            AuthResult authenticationResult = new AuthResult();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appsettings.JwtSettings.Secret);

                ClaimsIdentity subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("UserName",user.Name == null? "" :user.Name),
                    new Claim("EmailId",user.Email==null?"":user.Email),
                    new Claim("Phone", user.Phone!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _appsettings.JwtSettings.Audience,
                    Issuer = _appsettings.JwtSettings.Issuer,
                    Subject = subject,
                    Expires = DateTime.UtcNow.Add(_appsettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                authenticationResult.Token = tokenHandler.WriteToken(token);

                var refreshToken = new RefreshToken
                { 
                    Refreshtoken = Guid.NewGuid().ToString(),
                    Tokenid = token.Id,
                    Expiretime = DateTime.UtcNow.AddMonths(6),
                    UserId = user.UserId,
                };

                var existingRefreshToken = await _context.TblRefreshtokens.FirstOrDefaultAsync(rt => rt.Userid == user.UserId);
                if (existingRefreshToken != null)
                {
                    _context.TblRefreshtokens.Remove(existingRefreshToken);
                }

                TblRefreshtoken tblRefreshToken = _mapper.Map<RefreshToken,TblRefreshtoken>(refreshToken);
                await _context.TblRefreshtokens.AddAsync(tblRefreshToken);
                await _context.SaveChangesAsync();

                authenticationResult.RefreshToken = refreshToken.Refreshtoken;
                authenticationResult.Success = true;
                return authenticationResult;
            }
            catch(Exception ex)
            {
                authenticationResult.Token = null;
                authenticationResult.RefreshToken = null;
                authenticationResult.Success = false;
                authenticationResult.Errors.Append(ex.Message);
                return authenticationResult;
            }

        }

        public async Task<APIResponse<TokenModel>> RefreshTokenAsync(TokenModel request)
        {
            APIResponse<TokenModel> response = new APIResponse<TokenModel>();

            TblRefreshtoken? existingRefreshToken = await _context.TblRefreshtokens.FirstOrDefaultAsync(x => x.Refreshtoken == request.RefreshToken);

            if (existingRefreshToken == null || existingRefreshToken.Expiretime < DateTime.UtcNow)
            {
                response.ResponseCode = 404;
                response.Message = "Invalid or expired refresh token.";
                return response;
            }

            TblUser? user = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == existingRefreshToken.Userid);
            if (user == null)
            {
                response.ResponseCode = 404;
                response.Message = "User not found!";
                return response;
            }

            return await LoginAsync(new LoginModel { UserName = user.Username!, Password = user.Password! });
        }
    }
}