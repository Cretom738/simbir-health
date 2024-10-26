using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CreateHistoryDto
    {
        public required DateTime Date { get; set; }

        [Range(1, long.MaxValue)]
        public required long PacientId { get; set; }

        [Range(1, long.MaxValue)]
        public required long HospitalId { get; set; }

        [Range(1, long.MaxValue)]
        public required long DoctorId { get; set; }

        [Required(AllowEmptyStrings = false), MaxLength(200)]
        public required string Room { get; set; }

        [Required(AllowEmptyStrings = false)]
        public required string Data { get; set; }
    }
}
