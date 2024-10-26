using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class History
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("date")]
        public required DateTime Date { get; set; }

        [Column("pacient_id")]
        public required long PacientId { get; set; }

        [Column("hospital_id")]
        public required long HospitalId { get; set; }

        [Column("doctor_id")]
        public required long DoctorId { get; set; }

        [Column("room")]
        [MaxLength(200)]
        public required string Room { get; set; }

        [Column("data")]
        public required string Data { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
