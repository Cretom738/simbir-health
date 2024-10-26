using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("sessions")]
    public class Session
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("token_pair_id")]
        public required long TokenPairId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("expired_at")]
        public required DateTime ExpiredAt { get; set; }

        [Column("account_id")]
        public required long AccountId { get; set; }

        public Account Account { get; set; } = null!;
    }
}
