using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class UpdateAccountDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string LastName { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string FirstName { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Password { get; set; }

        [MinLength(1)]
        public required ISet<Role> Roles { get; set; }
    }
}