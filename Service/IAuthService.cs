using JwtUser.Helper;
using JwtUser.Modal;

namespace JwtUser.Service
{
    public interface IAuthService
    {
        Task<APIResponse<TokenModel>> LoginAsync(LoginModel login);
        Task<APIResponse<TokenModel>> RefreshTokenAsync(TokenModel login);
    }
}
