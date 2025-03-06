using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Context;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;

namespace TinProjektBackend.Repositories;

public interface ITripRepository
{
    Task<TripListResponseModel> GetAllTripsAsync(int pageNum, 
        int pageSize, 
        string? country, 
        int? categoryId, 
        decimal? priceMin, 
        decimal? priceMax,
        DateTime? startDate,
        DateTime? endDate,
        int? numOfParticipants);

    Task<TripResponseModel> GetTripByIdAsync(int id);
    Task AddTripAsync(Trip newTrip);
    Task UpdateTripAsync(int id, UpdateTripRequestModel trip);
    Task<bool> TripExists(int id);
    Task DeleteTripByIdAsync(int id);
}
public class TripRepository : ITripRepository
{
    private DatabaseContext Context { get; set; }

    public TripRepository(DatabaseContext context)
    {
        Context = context;
    }
public async Task<TripListResponseModel> GetAllTripsAsync(
    int pageNum,
    int pageSize,
    string? country,
    int? categoryId,
    decimal? priceMin,
    decimal? priceMax,
    DateTime? startDate,
    DateTime? endDate,
    int? numOfParticipants)
{
    var currentDate = startDate?.Date ?? DateTime.Now.Date;
    var endDateToUse = endDate?.Date ?? DateTime.Now.AddDays(365).Date;

    var query = Context.Trips
        .Include(t => t.TripCategory)
        .Include(t => t.TripSchedules)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(country))
    {
        query = query.Where(t => t.Country.Contains(country));
    }
    if (categoryId.HasValue)
    {
        query = query.Where(t => t.CategoryId == categoryId.Value);
    }
    if (priceMin.HasValue)
    {
        query = query.Where(t => t.Price >= priceMin.Value);
    }
    if (priceMax.HasValue)
    {
        query = query.Where(t => t.Price <= priceMax.Value);
    }

    query = query.Where(t => t.TripSchedules
        .Any(ts => ts.StartDate >= currentDate && ts.EndDate <= endDateToUse));

    if (numOfParticipants.HasValue)
    {
        query = query.Where(t => t.TripSchedules
            .Any(ts => ts.PlacesAvailable >= numOfParticipants.Value));
    }

    var trips = await query
        .OrderBy(t => t.Name)
        .Skip((pageNum - 1) * pageSize)
        .Take(pageSize)
        .Select(t => new TripResponseModel
        {
            TripId = t.TripId,
            Name = t.Name,
            Country = t.Country,
            TripCategory = t.TripCategory.CategoryName,
            Description = t.Description,
            Price = t.Price,
            ImagePath = t.ImageLink,
            StartDate = t.TripSchedules
                .Where(ts => ts.StartDate >= currentDate && ts.EndDate <= endDateToUse)
                .OrderBy(ts => ts.StartDate)
                .Select(ts => ts.StartDate)
                .FirstOrDefault(),
            EndDate = t.TripSchedules
                .Where(ts => ts.StartDate >= currentDate && ts.EndDate <= endDateToUse)
                .OrderBy(ts => ts.StartDate)
                .Select(ts => ts.EndDate)
                .FirstOrDefault(),
            AvailableSlots = t.TripSchedules
                .Where(ts => ts.StartDate >= currentDate && ts.EndDate <= endDateToUse)
                .OrderBy(ts => ts.StartDate)
                .Select(ts => ts.PlacesAvailable)
                .FirstOrDefault()
        })
        .ToListAsync();

    return new TripListResponseModel
    {
        Trips = trips
    };
}

    public async Task<TripResponseModel> GetTripByIdAsync(int id)
    {
        return await Context.Trips
            .Include(t => t.TripCategory)
            .Where(t => t.TripId == id)
            .Select(t => new TripResponseModel
            {
                TripId = id,
                Name = t.Name,
                Country = t.Country,
                TripCategory = t.TripCategory.CategoryName,
                Description = t.Description,
                Price = t.Price,
                ImagePath = t.ImageLink,
                StartDate = t.TripSchedules.FirstOrDefault().StartDate,
                EndDate = t.TripSchedules.FirstOrDefault().EndDate,
                AvailableSlots = t.TripSchedules.FirstOrDefault().PlacesAvailable
            })
            .FirstOrDefaultAsync();
    }

    public async Task AddTripAsync(Trip newTrip)
    {
        Context.Trips.AddAsync(newTrip);
        await Context.SaveChangesAsync();
    }
    
    public async Task UpdateTripAsync(int id, UpdateTripRequestModel trip)
    {
        var tripToUpdate = await Context.Trips.FirstOrDefaultAsync(t => t.TripId == id);
        if (tripToUpdate is null)
            throw new KeyNotFoundException("Trip not found.");

        if (!string.IsNullOrWhiteSpace(trip.Name))
        {
            tripToUpdate.Name = trip.Name;
        }

        if (!string.IsNullOrWhiteSpace(trip.Country))
        {
            tripToUpdate.Country = trip.Country;
        }

        if (!string.IsNullOrWhiteSpace(trip.Description))
        {
            tripToUpdate.Description = trip.Description;
        }

        if (!string.IsNullOrWhiteSpace(trip.ImagePath))
        {
            tripToUpdate.ImageLink = trip.ImagePath;
        }

        if (trip.Price.HasValue)
        {
            tripToUpdate.Price = trip.Price.Value;
        }
        Context.Trips.Update(tripToUpdate);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> TripExists(int id)
    {
        var trip = await Context.Trips.FindAsync(id);
        if (trip is null)
        {
            return false;
        }
        return true;
    }

    public async Task DeleteTripByIdAsync(int id)
    {
        var schedulesToDelete = Context.TripSchedules
            .Where(ts => ts.TripId == id && ts.StartDate > DateTime.Now)
            .ToList();

        if (schedulesToDelete.Any())
        {
            Context.TripSchedules.RemoveRange(schedulesToDelete);
        }
        await Context.SaveChangesAsync();
    }
}