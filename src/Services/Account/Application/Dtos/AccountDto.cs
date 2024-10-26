using Domain.Enums;

namespace Application.Dtos
{
    public class AccountDto
    {
        public required long Id { get; set; }

        public required string LastName { get; set; }

        public required string FirstName { get; set; }

        public required string Username { get; set; }

        public required ISet<Role> Roles { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }
    }
}
