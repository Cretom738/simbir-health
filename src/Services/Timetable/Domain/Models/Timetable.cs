using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Timetable
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("from")]
        public required DateTime From { get; set; }

        [Column("to")]
        public required DateTime To { get; set; }

        [Column("room")]
        [MaxLength(200)]
        public required string Room { get; set; }

        [Column("hospital_id")]
        public required long HospitalId { get; set; }

        [Column("doctor_id")]
        public required long DoctorId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public IList<Appointment> Appointments { get; set; } = [];
    }
}
