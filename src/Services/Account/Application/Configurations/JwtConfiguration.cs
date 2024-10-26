using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Configurations
{
    public class JwtConfiguration
    {
        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required string SigningKey { get; set; }

        [Range(0, long.MaxValue)]
        public required long AccessTokenLifeTime { get; set; }

        [Range(0, long.MaxValue)]
        public required long RefreshTokenLifeTime { get; set; }

        public DateTime AccessTokenExpirationDateTime => DateTime.UtcNow.AddSeconds(AccessTokenLifeTime);

        public DateTime RefreshTokenExpirationDateTime => DateTime.UtcNow.AddSeconds(RefreshTokenLifeTime);

        public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SigningKey));

        public TokenValidationParameters TokenValidationParameters => new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = SecurityKey
        };
    }
}
