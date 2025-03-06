using TinProjektBackend.Exceptions;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Repositories;

namespace TinProjektBackend.Services;

public interface ITripScheduleSerive
{
  Task AddNewScheduleToTripAsync(ScheduledTripRequestModel model);
}

public class TripScheduleService : ITripScheduleSerive
{
  private ITripScheduleRepository TripScheduleRepository { get; set; }
  private ITripRepository TripRepository { get; set; }

  public TripScheduleService(ITripScheduleRepository tripScheduleRepository, ITripRepository repository)
  {
    TripScheduleRepository = tripScheduleRepository;
    TripRepository = repository;
  }

  public async Task AddNewScheduleToTripAsync(ScheduledTripRequestModel model)
  {
    if (!await TripRepository.TripExists(model.TripId))
    {
      throw new BadRequestException("Wycieczka o takim ID nie istnieje!");
    }
    if (model.StartDate > model.EndDate)
    {
      throw new BadRequestException("Data poczatkowa pozniej niz koncowa!");
    }
    if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
    {
      throw new BadRequestException("Daty nie moga byc z przeszłości!");
    }

    await TripScheduleRepository.AddNewScheduleToTripAsync(model);
  }
}