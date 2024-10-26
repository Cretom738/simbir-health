using Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class UpdateHospitalDto
    {
        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Name { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Address { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string ContactPhone { get; set; }

        [MinLength(1)]
        [ArrayStringElementLength(200)]
        public required IList<string> Rooms { get; set; }
    }
}
