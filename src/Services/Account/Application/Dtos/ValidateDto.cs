using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ValidateDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(2000)]
        public required string AccessToken { get; set; }
    }
}
