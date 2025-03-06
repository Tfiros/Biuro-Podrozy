using TinProjektBackend.Exceptions;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;
using TinProjektBackend.Repositories;

namespace TinProjektBackend.Services;

public interface IOrderService
{
    Task PlaceOrderAsync(OrderRequestModel orderRequest, string userId);
    Task<OrderedTripsResponseModel> GetUsersOrdersAsync(string userLogin);
}
public class OrderService : IOrderService
{
    private readonly IOrderRepository OrderRepository;
    private readonly ITripScheduleRepository TripScheduleRepository;
    private readonly IUserRepository UserRepository;
    private readonly ITripRepository TripRepository;

    public OrderService(IOrderRepository orderRepository, ITripScheduleRepository tripScheduleRepository,
        IUserRepository userRepository, ITripRepository tripRepository)
    {
        OrderRepository = orderRepository;
        TripScheduleRepository = tripScheduleRepository;
        UserRepository = userRepository;
        TripRepository = tripRepository;
    }

    public async Task PlaceOrderAsync(OrderRequestModel orderRequest, string userId)
    {
        var user = await UserRepository.GetUserByLoginAsync(userId);
        if (user is null )
        {
            throw new BadRequestException("Uzytkownik o takim loginie nie istnieje");
        }

        if (!await TripRepository.TripExists(orderRequest.TripID))
        {
            throw new BadRequestException("Wycieczka o takim id nie istnieje");
        }
        var tripSchedule = await TripScheduleRepository.FindMatchingScheduleAsync(
            orderRequest.TripID,
            orderRequest.StartDate,
            orderRequest.EndDate);

        if (tripSchedule is null)
        {
            throw new KeyNotFoundException("Nie znaleziono tej wycieczki w podanym czasie");
        }
        if (tripSchedule.PlacesAvailable < orderRequest.NumOfParticipants)
            throw new InvalidOperationException("Brak miejsc w wybranej wycieczce dla tej ilosci osob!");

        await OrderRepository.PlaceOrderAsync(orderRequest, user.UserId, tripSchedule);
    }

    public async Task<OrderedTripsResponseModel> GetUsersOrdersAsync(string userLogin)
    {
        var user = await UserRepository.GetUserByLoginAsync(userLogin);
        if (user == null)
        {
            throw new BadRequestException("Uzytkownik o takim loginie nie istnieje");
        }

        var orderedTrips = await OrderRepository.GetUsersOrdersAsync(user.UserId);
        return orderedTrips;
    }

}