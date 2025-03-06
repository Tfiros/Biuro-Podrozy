namespace TinProjektBackend.Models.ResponseModels;

public class OrderedTripsResponseModel
{
    public List<OrderedTripResModel> OrderedTrips;
}

public class OrderedTripResModel
{
    public int TripId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string TripCategory { get; set; }
    public string ImagePath { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public decimal Price { get; set; }
}