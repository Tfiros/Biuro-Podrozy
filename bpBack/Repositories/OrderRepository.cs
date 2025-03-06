using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Context;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;

namespace TinProjektBackend.Repositories;

public interface IOrderRepository
{
    Task PlaceOrderAsync(OrderRequestModel request, int userId, TripSchedule tripSchedule);
    Task<OrderedTripsResponseModel> GetUsersOrdersAsync(int userId);
}
public class OrderRepository : IOrderRepository
{
    private DatabaseContext Context { get; set; }

    public OrderRepository(DatabaseContext context)
    {
        Context = context;
    }

    public async Task PlaceOrderAsync(OrderRequestModel request, int userId, TripSchedule tripSchedule)
    {
        var trip = await Context.Trips.FirstOrDefaultAsync(t => t.TripId == request.TripID);
        
        using var transaction = await Context.Database.BeginTransactionAsync();
        
        try
        {
            tripSchedule.PlacesAvailable -= request.NumOfParticipants;
            Context.TripSchedules.Update(tripSchedule);

            var newOrder = new Order
            {
                UserId = userId,
                ScheduledTripId = tripSchedule.ScheduleId,
                NumOfParticipants = request.NumOfParticipants,
                IssueDate = DateTime.Now,
                EndDate = request.EndDate,
                TotalPrice = request.NumOfParticipants * trip.Price
            };
            Context.Orders.Add(newOrder);

            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw new Exception("Cos poszlo nie tak podczas dodawania zamowienia!");
        }
    }

    public async Task<OrderedTripsResponseModel> GetUsersOrdersAsync(int userId)
    {
        var list = await Context.Orders
            .Where(order => order.UserId == userId)
            .Include(order => order.TripSchedule)
            .ThenInclude(schedule => schedule.Trip)
            .ThenInclude(trip => trip.TripCategory)
            .Select(order => new OrderedTripResModel
            {
                TripId = order.TripSchedule.Trip.TripId,
                Name = order.TripSchedule.Trip.Name,
                Country = order.TripSchedule.Trip.Country,
                TripCategory = order.TripSchedule.Trip.TripCategory.CategoryName,
                ImagePath = order.TripSchedule.Trip.ImageLink,
                startDate = order.TripSchedule.StartDate,
                endDate = order.TripSchedule.EndDate,
                Price = order.TotalPrice
            })
            .ToListAsync();
        return new OrderedTripsResponseModel()
        {
            OrderedTrips = list
        };
    }
}