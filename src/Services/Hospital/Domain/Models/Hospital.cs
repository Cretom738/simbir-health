using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [Table("hospitals")]
    public class Hospital
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public required string Name { get; set; }

        [Column("address")]
        [MaxLength(200)]
        public required string Address { get; set; }

        [Column("contact_phone")]
        [MaxLength(50)]
        public required string ContactPhone { get; set; }

        [Column("rooms")]
        public required IList<string> Rooms { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
