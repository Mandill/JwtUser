﻿namespace JwtUser.Modal
{
    public class ServiceConfiguration
    {
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
