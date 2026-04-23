using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.AirlineService.Entities;
using SkyBooker.AirlineService.Interfaces;

namespace SkyBooker.AirlineService.Controllers;

[ApiController]
[Route("api/airlines")]
public class AirlineController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirlineController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAirlines()
    {
        var airlines = await _airlineService.GetAllAirlinesAsync();
        return Ok(airlines);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAirlineById(int id)
    {
        var airline = await _airlineService.GetAirlineByIdAsync(id);
        if (airline == null) return NotFound();
        return Ok(airline);
    }

    [HttpGet("{iata}")]
    public async Task<IActionResult> GetAirlineByIata(string iata)
    {
        var airline = await _airlineService.GetAirlineByIataAsync(iata);
        if (airline == null) return NotFound();
        return Ok(airline);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAirline([FromBody] Airline airline)
    {
        var created = await _airlineService.CreateAirlineAsync(airline);
        return CreatedAtAction(nameof(GetAirlineById), new { id = created.AirlineId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAirline(int id, [FromBody] Airline airline)
    {
        try
        {
            var updated = await _airlineService.UpdateAirlineAsync(id, airline);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateAirline(int id)
    {
        await _airlineService.DeactivateAirlineAsync(id);
        return NoContent();
    }
}
