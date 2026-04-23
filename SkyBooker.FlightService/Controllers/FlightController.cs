using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.FlightService.Entities;
using SkyBooker.FlightService.Interfaces;

namespace SkyBooker.FlightService.Controllers;

[ApiController]
[Route("api/flights")]
[Authorize]
public class FlightController : ControllerBase
{
    private readonly IFlightService _service;

    public FlightController(IFlightService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Flight flight)
    {
        await _service.AddFlight(flight);
        return Ok("Flight added successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetFlightById(id);
        return Ok(result);
    }

    [HttpGet("number/{flightNumber}")]
    public async Task<IActionResult> GetByNumber(string flightNumber)
    {
        var result = await _service.GetFlightByNumber(flightNumber);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string origin, string destination, DateTime date)
    {
        var result = await _service.SearchFlights(origin, destination, date);
        return Ok(result);
    }

    [HttpGet("roundtrip")]
    public async Task<IActionResult> RoundTrip(string origin, string destination, DateTime depart, DateTime ret)
    {
        var result = await _service.SearchRoundTrip(origin, destination, depart, ret);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Flight flight)
    {
        await _service.UpdateFlight(flight);
        return Ok("Updated successfully");
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _service.UpdateStatus(id, status);
        return Ok("Status updated");
    }

    [HttpPut("decrement")]
    public async Task<IActionResult> Decrement(int id, int count)
    {
        await _service.DecrementSeats(id, count);
        return Ok("Seats decremented");
    }

    [HttpPut("increment")]
    public async Task<IActionResult> Increment(int id, int count)
    {
        await _service.IncrementSeats(id, count);
        return Ok("Seats incremented");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteFlight(id);
        return Ok("Deleted successfully");
    }

    [HttpGet("airline/{airlineId}")]
    public async Task<IActionResult> GetByAirline(int airlineId)
    {
        var result = await _service.GetFlightsByAirline(airlineId);
        return Ok(result);
    }
}