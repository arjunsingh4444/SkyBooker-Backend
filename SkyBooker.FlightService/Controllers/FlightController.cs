using Microsoft.AspNetCore.Mvc;
using SkyBooker.FlightService.DTOs;
using SkyBooker.FlightService.Interfaces;

namespace SkyBooker.FlightService.Controllers;

[ApiController]
[Route("api/flights")]
public class FlightController : ControllerBase
{
    private readonly IFlightService _service;

    public FlightController(IFlightService service)
    {
        _service = service;
    }

    //  POST: Add Flight
    [HttpPost]
    public async Task<IActionResult> Add(FlightDto dto)
    {
        await _service.AddFlight(dto);
        return Ok("Flight added successfully");
    }

    // GET: By Flight ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var flight = await _service.GetFlightById(id);

        if (flight == null)
            return NotFound("Flight not found");

        return Ok(flight);
    }

    //  GET: By Flight Number
    [HttpGet("number/{flightNumber}")]
    public async Task<IActionResult> GetByNumber(string flightNumber)
    {
        var flight = await _service.GetFlightByNumber(flightNumber);

        if (flight == null)
            return NotFound("Flight not found");

        return Ok(flight);
    }

    //  GET: Search One Way
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string origin,
        string destination,
        DateTime date)
    {
        var flights = await _service.SearchFlights(origin, destination, date);

        return Ok(flights);
    }

    //  GET: Round Trip Search
    [HttpGet("roundtrip")]
    public async Task<IActionResult> RoundTrip(
        string origin,
        string destination,
        DateTime departDate,
        DateTime returnDate)
    {
        var result = await _service.SearchRoundTrip(origin, destination, departDate, returnDate);

        return Ok(result);
    }

    // PUT: Update Flight
    [HttpPut]
    public async Task<IActionResult> Update(FlightDto dto)
    {
        await _service.UpdateFlight(dto);
        return Ok("Flight updated successfully");
    }

    // PUT: Update Status
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(int flightId, string status)
    {
        await _service.UpdateStatus(flightId, status);
        return Ok("Status updated successfully");
    }

    // PUT: Decrement Seats
    [HttpPut("decrement-seats")]
    public async Task<IActionResult> DecrementSeats(int flightId, int count)
    {
        await _service.DecrementSeats(flightId, count);
        return Ok("Seats decremented");
    }

    // PUT: Increment Seats
    [HttpPut("increment-seats")]
    public async Task<IActionResult> IncrementSeats(int flightId, int count)
    {
        await _service.IncrementSeats(flightId, count);
        return Ok("Seats incremented");
    }

    //  DELETE: Delete Flight
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteFlight(id);
        return Ok("Flight deleted successfully");
    }

    // GET: Flights by Airline
    [HttpGet("airline/{airlineId}")]
    public async Task<IActionResult> GetByAirline(int airlineId)
    {
        var flights = await _service.GetFlightsByAirline(airlineId);
        return Ok(flights);
    }
}