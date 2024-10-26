namespace Application.Dtos
{
    public class TimetableDto
    {
        public required long Id { get; set; }

        public required DateTime From { get; set; }

        public required DateTime To { get; set; }

        public required string Room { get; set; }

        public required long HospitalId { get; set; }

        public required long DoctorId { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }
    }
}
