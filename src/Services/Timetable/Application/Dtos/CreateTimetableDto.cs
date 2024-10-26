using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CreateTimetableDto
    {
        public required DateTime From { get; set; }

        public required DateTime To { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Room { get; set; }

        [Range(1, long.MaxValue)]
        public required long HospitalId { get; set; }

        [Range(1, long.MaxValue)]
        public required long DoctorId { get; set; }
    }
}
