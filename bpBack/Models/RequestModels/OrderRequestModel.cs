using System.ComponentModel.DataAnnotations;

namespace TinProjektBackend.Models.RequestModels;

public class OrderRequestModel
{
    [Required]
    public int TripID { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int NumOfParticipants { get; set; }
}