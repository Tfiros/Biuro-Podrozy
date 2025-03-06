using TinProjektBackend.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Services;

namespace TinProjektBackend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TripScheduleController : ControllerBase
{
    public ITripScheduleSerive TripScheduleSerive { get; set; }

    public TripScheduleController(ITripScheduleSerive tripScheduleSerive)
    {
        TripScheduleSerive = tripScheduleSerive;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddNewScheduleToTripAsync(ScheduledTripRequestModel scheduleForTrip)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await TripScheduleSerive.AddNewScheduleToTripAsync(scheduleForTrip);
            return Created("", new
            {
                Message = "Schedule added successfully.",
                ScheduleDetails = scheduleForTrip
            });
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Błąd podczas dodawania wycieczki: ", Details = ex.Message });
        }
    }
}