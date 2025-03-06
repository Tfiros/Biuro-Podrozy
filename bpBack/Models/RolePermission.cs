using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
namespace TinProjektBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;
[Table("RolePermissions")]
[PrimaryKey("RoleId","PermissionId")]
public class RolePermission
{
    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Permission_ID")]
    public int PermissionId { get; set; }
    
    [JsonIgnore]
    [ForeignKey("RoleId")]
    public virtual UserRole Role { get; set; }
    [JsonIgnore]
    [ForeignKey("PermissionId")]
    public virtual Permission Permission { get; set; }
}
