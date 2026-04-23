using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.AirlineService.Entities;
using SkyBooker.AirlineService.Interfaces;

namespace SkyBooker.AirlineService.Controllers;

[ApiController]
[Route("api/airports")]
public class AirportController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirportController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAirports([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query parameter is required");
        }
        var airports = await _airlineService.SearchAirportsAsync(query);
        return Ok(airports);
    }

    [HttpGet("{iata}")]
    public async Task<IActionResult> GetAirportByIata(string iata)
    {
        var airport = await _airlineService.GetAirportByIataAsync(iata);
        if (airport == null) return NotFound();
        return Ok(airport);
    }

    [HttpGet("city/{city}")]
    public async Task<IActionResult> GetAirportsByCity(string city)
    {
        var airports = await _airlineService.GetAirportsByCityAsync(city);
        return Ok(airports);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAirport([FromBody] Airport airport)
    {
        var created = await _airlineService.CreateAirportAsync(airport);
        return CreatedAtAction(nameof(GetAirportByIata), new { iata = created.IataCode }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAirport(int id, [FromBody] Airport airport)
    {
        try
        {
            var updated = await _airlineService.UpdateAirportAsync(id, airport);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
