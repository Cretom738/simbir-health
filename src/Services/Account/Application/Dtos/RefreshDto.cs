using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class RefreshDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(2000)]
        public required string RefreshToken { get; set; }
    }
}
