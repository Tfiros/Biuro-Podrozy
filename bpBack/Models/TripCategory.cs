using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TinProjektBackend.Models;
[Table("TripCategories")]
public class TripCategory
{
    [Key]
    [Column("Category_ID")]
    public int CategoryId { get; set; }

    [Required]
    [Column("Category_Name")]
    [MaxLength(50)]
    public string CategoryName { get; set; }
    [JsonIgnore]
    public virtual List<Trip> Trips { get; set; }
}