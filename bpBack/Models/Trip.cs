using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;
[Table("Trips")]
public class Trip
{
    [Key]
    [Column("Trip_ID")]
    public int TripId { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }    

    [Required]
    [Column("Category_ID")]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Country { get; set; }
    
    [Required]
    public string Description { get; set; }

    [Required]
    [Column("Image_link")]
    public string ImageLink { get; set; }
    [Required]
    [Column("Price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
   [ForeignKey("CategoryId")]
   [JsonIgnore]
    public virtual TripCategory TripCategory { get; set; }
    [JsonIgnore]

    public virtual List<TripSchedule> TripSchedules { get; set; }
}