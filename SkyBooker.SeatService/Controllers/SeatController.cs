using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.SeatService.Interfaces;

namespace SkyBooker.SeatService.Controllers;

[ApiController]
[Route("api/seats")]
[Authorize]
public class SeatController : ControllerBase
{
    private readonly ISeatService _service;

    public SeatController(ISeatService service)
    {
        _service = service;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(int flightId, int rows, int seatsPerRow, string seatClass)
    {
        await _service.AddSeatsForFlight(flightId, rows, seatsPerRow, seatClass);
        return Ok();
    }

    [HttpGet("available")]
    public async Task<IActionResult> Available(int flightId)
        => Ok(await _service.GetAvailableSeats(flightId));

    [HttpPut("hold/{id}")]
    public async Task<IActionResult> Hold(int id)
    {
        await _service.HoldSeat(id);
        return Ok("Held");
    }

    [HttpPut("release/{id}")]
    public async Task<IActionResult> Release(int id)
    {
        await _service.ReleaseSeat(id);
        return Ok("Released");
    }

    [HttpPut("confirm/{id}")]
    public async Task<IActionResult> Confirm(int id)
    {
        await _service.ConfirmSeat(id);
        return Ok("Confirmed");
    }

    [HttpGet("map/{flightId}")]
    public async Task<IActionResult> Map(int flightId)
        => Ok(await _service.GetSeatMap(flightId));

    [HttpGet("count")]
    public async Task<IActionResult> Count(int flightId, string seatClass)
        => Ok(await _service.CountAvailableByClass(flightId, seatClass));

    [HttpDelete("{flightId}")]
    public async Task<IActionResult> Delete(int flightId)
    {
        await _service.DeleteSeatsForFlight(flightId);
        return Ok();
    }
}