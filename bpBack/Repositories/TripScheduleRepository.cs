using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Context;
using TinProjektBackend.Models.RequestModels;

namespace TinProjektBackend.Repositories;

public interface ITripScheduleRepository
{
    Task AddNewScheduleToTripAsync(ScheduledTripRequestModel model);
    Task<TripSchedule?> FindMatchingScheduleAsync(int tripId, DateTime startDate, DateTime endDate);

}
public class TripScheduleRepository : ITripScheduleRepository
{
    private DatabaseContext Context { get; set; }

    public TripScheduleRepository(DatabaseContext context)
    {
        Context = context;
    }

    public async Task AddNewScheduleToTripAsync(ScheduledTripRequestModel model)
    {
        var newSchedule = new TripSchedule()
        {
            TripId = model.TripId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            PlacesAvailable = model.PlacesAvailable
        };
       Context.TripSchedules.Add(newSchedule);
       await Context.SaveChangesAsync();
    }
    public async Task<TripSchedule?> FindMatchingScheduleAsync(int tripId, DateTime startDate, DateTime endDate)
    {
        return await Context.TripSchedules
            .Where(ts => ts.TripId == tripId && 
                         ts.StartDate >= startDate && 
                         ts.EndDate <= endDate)
            .FirstOrDefaultAsync();
    }
}