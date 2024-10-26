using Domain.Enums;

namespace Application.Classes.Data
{
    public record JwtPayloadData(long SessionId, long AccountId, long TokenPairId, IList<Role> Roles, JwtTokenType TokenType);
}
