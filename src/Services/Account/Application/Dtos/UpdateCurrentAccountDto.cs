using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class UpdateCurrentAccountDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string LastName { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string FirstName { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Password { get; set; }
    }
}