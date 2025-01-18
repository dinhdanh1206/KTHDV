using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthAPI.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("idUser")]
        public int IdUser { get; set; }

        [Required]
        [Column("userName")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [Column("email")]
        [StringLength(255)]
        public string? Email { get; set; }

        [Required]
        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = "user";
    }
}
