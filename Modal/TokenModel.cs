using Newtonsoft.Json;

namespace JwtUser.Modal
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class AuthResult : TokenModel
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

}
