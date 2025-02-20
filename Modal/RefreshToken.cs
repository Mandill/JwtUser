namespace JwtUser.Modal
{
    public class RefreshToken
    {
        public string UserId{ get; set; }
        public string? Tokenid { get; set; }
        public string? Refreshtoken { get; set; }
        public DateTime? Expiretime { get; set; }
        public bool isExpired => Expiretime <= DateTime.UtcNow ;
    }
}
