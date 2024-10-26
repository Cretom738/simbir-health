using Application.Classes.Data;
using Domain.Enums;

namespace Application.Services
{
    public interface IJwtService
    {
        string GenerateToken(GenerateJwtData data, JwtTokenType type);

        JwtPairData GenerateTokenPair(GenerateJwtData data);

        Task<JwtPayloadData?> VerifyToken(string token, JwtTokenType type);

        Task BlacklistTokenAsync(long tokenPairId);
    }
}
