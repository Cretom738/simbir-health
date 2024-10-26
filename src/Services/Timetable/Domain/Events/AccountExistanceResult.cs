using Domain.Enums;

namespace Domain.Events
{
    public record AccountExistanceResult(IList<Role> Roles);
}
