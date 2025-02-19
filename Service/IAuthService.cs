using JwtUser.Helper;
using JwtUser.Modal;

namespace JwtUser.Service
{
    public interface IAuthService
    {
        Task<APIResponse<TokenModel>> LoginAsync(LoginModel login);
    }
}
