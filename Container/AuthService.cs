using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Azure;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using JwtUser.Service;
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
            APIResponse<TokenModel> apiresponse = new APIResponse<TokenModel>();
            TblUser user = _context.TblUsers.FirstOrDefault(x => x.Username == login.UserName && x.Password == login.Password);
            if (user == null)
            {
                apiresponse.ResponseCode = 404;
                apiresponse.Message = "Invalid username or password";
                return apiresponse;
            }
            UserModel userModel = _mapper.Map<TblUser, UserModel>(user!);
            AuthResult authenticationResult = await AuthenticateAsync(userModel);

            if (authenticationResult != null && authenticationResult.Success)
            {
                apiresponse.Result = new TokenModel() { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken };
                apiresponse.Message = "Success";
            }
            return apiresponse;
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
                    Subject = subject,
                    Expires = DateTime.UtcNow.Add(_appsettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenId = token.Id;
                authenticationResult.Token = tokenHandler.WriteToken(token);
                authenticationResult.RefreshToken = null;
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
    }
}