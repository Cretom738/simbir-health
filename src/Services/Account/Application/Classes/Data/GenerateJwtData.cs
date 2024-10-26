using Domain.Enums;

namespace Application.Classes.Data
{
    public record GenerateJwtData(long SessionId, long AccountId, long TokenPairId, IList<Role> Roles);
}
