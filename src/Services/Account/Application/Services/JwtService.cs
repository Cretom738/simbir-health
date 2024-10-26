using Application.Classes.Constants;
using Application.Classes.Data;
using Application.Configurations;
using Domain.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;

        private readonly JwtConfiguration _jwtConfiguration;

        private readonly IDistributedCache _cache;

        public JwtService(
            IOptions<JwtConfiguration> jwtConfiguration,
            IDistributedCache cache)
        {
            _jwtTokenHandler = new JwtSecurityTokenHandler();
            _jwtConfiguration = jwtConfiguration.Value;
            _cache = cache;
        }

        public string GenerateToken(GenerateJwtData data, JwtTokenType type)
        {
            IList<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, data.AccountId.ToString(), ClaimValueTypes.Integer64),
                new Claim(CustomClaimTypes.SessionId, data.SessionId.ToString(), ClaimValueTypes.Integer64),
                new Claim(CustomClaimTypes.TokenPairId, data.TokenPairId.ToString(), ClaimValueTypes.Integer64),
                new Claim(CustomClaimTypes.TokenType, Enum.GetName(type)!),
            ];

            foreach (Role role in data.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(role)!));
            }

            return _jwtTokenHandler.WriteToken(new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: type == JwtTokenType.Access 
                    ? _jwtConfiguration.AccessTokenExpirationDateTime
                    : _jwtConfiguration.RefreshTokenExpirationDateTime,
                signingCredentials: new SigningCredentials(
                    _jwtConfiguration.SecurityKey, SecurityAlgorithms.HmacSha256)));
        }

        public JwtPairData GenerateTokenPair(GenerateJwtData data)
        {
            string accessToken = GenerateToken(data, JwtTokenType.Access);

            string refreshToken = GenerateToken(data, JwtTokenType.Refresh);

            return new JwtPairData(refreshToken, accessToken);
        }

        public async Task<JwtPayloadData?> VerifyToken(string token, JwtTokenType type)
        {
            TokenValidationResult validationResult = await _jwtTokenHandler
                .ValidateTokenAsync(token, _jwtConfiguration.TokenValidationParameters);

            if (!validationResult.IsValid)
            {
                return null;
            }

            IDictionary<string, object> claims = validationResult.Claims;

            long tokenPairId = Convert.ToInt64(claims[CustomClaimTypes.TokenPairId]);

            if (await IsTokenBlacklistedAsync(tokenPairId))
            {
                return null;
            }

            IList<Role> roles = claims
                .Where(kv => kv.Key == ClaimTypes.Role)
                .Select(kv => Enum.Parse<Role>(kv.Value.ToString()!))
                .ToList();

            return new JwtPayloadData(
                SessionId: Convert.ToInt64(claims[CustomClaimTypes.SessionId]),
                AccountId: Convert.ToInt64(claims[ClaimTypes.NameIdentifier]),
                tokenPairId,
                roles,
                TokenType: Enum.Parse<JwtTokenType>(claims[CustomClaimTypes.TokenType].ToString()!)
            );
        }

        public async Task BlacklistTokenAsync(long tokenPairId)
        {
            await _cache.SetStringAsync(GetBlacklistTokenKey(tokenPairId), tokenPairId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = _jwtConfiguration.RefreshTokenExpirationDateTime
            });
        }

        private async Task<bool> IsTokenBlacklistedAsync(long tokenPairId)
        {
            return await _cache.GetStringAsync(GetBlacklistTokenKey(tokenPairId)) != null;
        }

        private string GetBlacklistTokenKey(long tokenPairId)
        {
            return $"token-pair-id-{tokenPairId}";
        }
    }
}
