using System.ComponentModel.DataAnnotations;

namespace TinProjektBackend.Models.RequestModels;

public class ScheduledTripRequestModel
{
    [Required]
    public int TripId { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int PlacesAvailable { get; set; }
}