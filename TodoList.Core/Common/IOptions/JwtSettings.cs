﻿namespace TodoList.Core.Common.IOptions
{
    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
        public required int ExpirationInMinutes { get; set; }
    }
}
