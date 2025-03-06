using TinProjektBackend.Exceptions;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;
using TinProjektBackend.Repositories;

namespace TinProjektBackend.Services;

public interface ITripService
{
    Task<TripListResponseModel> GetAllTripsAsync(int pageNum, 
        int pageSize, 
        string? country, 
        int? categoryId, 
        decimal? priceMin, 
        decimal? priceMax,
        DateTime? startDate,
        DateTime? EndDate,
        int? numOfParticipants);

    Task<TripResponseModel> GetTripByIdAsync(int id);
    Task AddTripAsync(Trip newTrip);
    Task UpdateTripAsync(int id, UpdateTripRequestModel updatedTrip);
    Task DeleteTripByIdAsync(int id);

}
public class TripService : ITripService
{
    public ITripRepository TripRepository { get; set; }

    public TripService(ITripRepository tripRepository)
    {
        TripRepository = tripRepository;
    }

    public async Task<TripListResponseModel> GetAllTripsAsync(int pageNum, 
        int pageSize, 
        string? country, 
        int? categoryId, 
        decimal? priceMin, 
        decimal? priceMax,
        DateTime? startDate,
        DateTime? EndDate,
        int? numOfParticipants)
    {
        if (pageNum <= 0 || pageSize <= 0)
        {
            throw new ArgumentException("Pages are equal or below zero!");
        }

        return await TripRepository.GetAllTripsAsync(
            pageNum,
            pageSize,
            country,
            categoryId,
            priceMin, priceMax,
            startDate,
            EndDate,
            numOfParticipants);
    }

    public async Task<TripResponseModel> GetTripByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Nieporawne id wycieczki!");
        }

        var res = await TripRepository.GetTripByIdAsync(id);

        if (res is null)
        {
            throw new NotFoundException($"Trip by {id} does not exist.");
        }

        return res;
    }

    public async Task AddTripAsync(Trip newTrip)
    {
      
        await TripRepository.AddTripAsync(newTrip);
    }

    public async Task UpdateTripAsync(int id, UpdateTripRequestModel updatedTrip)
    {
        await TripRepository.UpdateTripAsync(id, updatedTrip);
        
    }

    public async Task DeleteTripByIdAsync(int id)
    {
        var trip = await TripRepository.TripExists(id);
        if (!trip)
        {
            throw new NotFoundException("Wycieczka nie znaleziona.");
        }
        await TripRepository.DeleteTripByIdAsync(id);
    }
}