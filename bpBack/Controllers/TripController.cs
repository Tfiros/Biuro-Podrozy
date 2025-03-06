using TinProjektBackend.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Services;

namespace TinProjektBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllTripsAsync(int pageNum, 
        int pageSize, 
        string? country, 
        int? categoryId, 
        decimal? priceMin, 
        decimal? priceMax,
        DateTime? startDate,
        DateTime? endDate,
        int? numOfParticipants
            )
    {
        try
        {
            var trips = await _tripService.GetAllTripsAsync(pageNum,
                pageSize,
                country,
                categoryId,
                priceMin,
                priceMax,startDate,
                endDate,numOfParticipants);
            return Ok(trips);

        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error while proceeding: " + e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        try
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            return Ok(trip);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error while proceeding: " + e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] AddTripRequestModel newTrip)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState + " nie poprawny requestModel!");
        }

        var trip = new Trip
        {
            Name = newTrip.Name,
            CategoryId = newTrip.CategoryId,
            Country = newTrip.Country,
            Description = newTrip.Description,
            ImageLink = newTrip.ImageLink,
            Price = newTrip.Price
        };

        try
        {
            await _tripService.AddTripAsync(trip);
            return CreatedAtAction(nameof(GetTripById), new { id = trip.TripId },
                new { Message = "Wycieczka dodana z sukcesem", TripId = trip.TripId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Błąd podczas dodawania wycieczki: ", Details = ex.Message });
        }
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTripAsync(int id, [FromBody] UpdateTripRequestModel updatedTrip)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model nie poprawny");
            return BadRequest(ModelState + " Model nie poprawny!");
        }
        
        try
        {
            await _tripService.UpdateTripAsync(id, updatedTrip);
            return Ok("Wycieczka zedytowana z sukcesem.");
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Blad podczas edycji wycieczki: ", Details = ex.Message });
        }
        
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTripAsync(int id)
    {
        try
        {
            await _tripService.DeleteTripByIdAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the trip.", Details = ex.Message });
        }
    }
}
