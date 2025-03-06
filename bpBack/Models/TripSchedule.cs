using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TinProjektBackend.Models;

[Table("Trip_Schedule")]
public class TripSchedule
{
    [Key]
    [Column("Schedule_ID")]
    public int ScheduleId { get; set; }

    [Required]
    [Column("Trip_ID")]
    public int TripId { get; set; }

    [Required]
    [Column("Start_date")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("End_date")]
    public DateTime EndDate { get; set; }

    [Required]
    [Column("Max_participants")]
    public int PlacesAvailable { get; set; }
    [JsonIgnore]
    [ForeignKey("TripId")]
    public virtual Trip Trip { get; set; }
    [JsonIgnore]
    public virtual List<Order> Orders { get; set; }
}