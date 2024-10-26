using Domain.Enums;

namespace Application.Dtos
{
    public class ValidationResultDto
    {
        public required long AccountId { get; set; }

        public required IList<Role> Roles { get; set; }
    }
}
