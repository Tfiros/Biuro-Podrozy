using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;

[Table("Users")]
public class User
{
    [Key]
    [Column("User_ID")]
    public int UserId { get; set; }

    [Required]
    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [MaxLength(50)]
    public string Password { get; set; }

    [Required]
    [Column("Refresh_token")]
    public string RefreshToken { get; set; }

    [Required]
    [Column("Expiration_date")]
    public DateTime ExpirationDate { get; set; }

    [Required]
    public string Salt { get; set; }
    [ForeignKey("RoleId")]
    [JsonIgnore]
    public virtual UserRole Role { get; set; }
    [JsonIgnore]
    public virtual Client Client { get; set; }
}