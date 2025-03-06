namespace TinProjektBackend.Models.ResponseModels;

public class TripResponseModel
{
    public int TripId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string TripCategory { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AvailableSlots { get; set; }
}