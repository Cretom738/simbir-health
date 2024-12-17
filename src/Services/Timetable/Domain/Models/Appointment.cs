using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("appointments")]
    public class Appointment
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("time")]
        public required DateTime Time { get; set; }

        [Column("patient_id")]
        public required long PatientId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("timetable_id")]
        public required long TimetableId { get; set; }

        public Timetable Timetable { get; set; } = null!;
    }
}