using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;

[Table("Permissions")]
public class Permission
{
    [Key]
    [Column("Permission_ID")]
    public int PermissionId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("Permission_Name")]
    public string PermissionName { get; set; }
    [JsonIgnore]
    public virtual List<RolePermission> RolePermissions { get; set; }
}