using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;
[Table("UserRoles")]
public class UserRole
{
    [Key]
    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("Role_Name")]
    public string RoleName { get; set; }
    
    [JsonIgnore]
    public virtual List<User> Users { get; set; }
    [JsonIgnore]
    public virtual List<RolePermission> RolePermissions { get; set; }
}