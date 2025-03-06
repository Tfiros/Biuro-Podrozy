using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TinProjektBackend.Models;
[Table("Orders")]
public class Order
{
    [Key]
    [Column("Order_ID")]
    public int OrderId { get; set; }

    [Required]
    [Column("User_ID")]
    public int UserId { get; set; }

    [Required]
    [Column("Scheduled_Trip_ID")]
    public int ScheduledTripId { get; set; }

    [Required]
    [Column("Issue_Date")]
    public DateTime IssueDate { get; set; }

    [Required]
    [Column("End_Date")]
    public DateTime EndDate { get; set; }

    [Required]
    [Column("Total_Price", TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }


    [Required]
    [Column("Number_of_participants")]
    public int NumOfParticipants { get; set; }
    [JsonIgnore]
    [ForeignKey("UserId")]
    public virtual Client Client { get; set; }
    [JsonIgnore]
    [ForeignKey("ScheduledTripId")]
    public virtual TripSchedule TripSchedule { get; set; }
}