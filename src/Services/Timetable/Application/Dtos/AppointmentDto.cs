namespace Application.Dtos
{
    public class AppointmentDto
    {
        public required long Id { get; set; }

        public required DateTime Time { get; set; }

        public required long PatientId { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }
    }
}
