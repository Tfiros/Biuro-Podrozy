using System.ComponentModel.DataAnnotations;

namespace TinProjektBackend.Models.RequestModels;

public class UpdateTripRequestModel
{
    public string? Name { get; set; }

    [Required]
    public int TripCategory { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public decimal? Price { get; set; }

}