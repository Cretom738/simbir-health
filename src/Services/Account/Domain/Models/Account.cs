using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("last_name")]
        [MaxLength(200)]
        public required string LastName { get; set; }

        [Column("first_name")]
        [MaxLength(200)]
        public required string FirstName { get; set; }

        [Column("username")]
        [MaxLength(200)]
        public required string Username { get; set; }

        [Column("password")]
        public required string Password { get; set; }

        [Column("roles")]
        public required IList<Role> Roles { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public IList<Session> Sessions { get; set; } = [];
    }
}
