namespace Application.Dtos
{
    public class HospitalDto
    {
        public required long Id { get; set; }

        public required string Name { get; set; }

        public required string Address { get; set; }

        public required string ContactPhone { get; set; }

        public required IList<string> Rooms { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }
    }
}
