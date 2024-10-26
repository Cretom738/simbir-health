using Domain.Enums;

namespace Domain.Events
{
    public record AccountDeleted(long AccountId, IList<Role> Roles);
}
