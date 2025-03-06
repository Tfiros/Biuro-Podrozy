using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;
[Table("Clients")]
public class Client
{
    [Key]
    [Column("User_ID")]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(100)]
    public string Address { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [ForeignKey("UserId")]
    [JsonIgnore]
    public virtual User User { get; set; }
    [JsonIgnore]
    public virtual List<Order> Orders { get; set; }
}