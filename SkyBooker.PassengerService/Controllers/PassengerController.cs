using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Interfaces;

namespace SkyBooker.PassengerService.Controllers;

[ApiController]
[Route("api/passengers")]
[Authorize]
public class PassengerController : ControllerBase
{
    private readonly IPassengerService _service;

    public PassengerController(IPassengerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(PassengerDto dto)
    {
        await _service.Add(dto);
        return Ok("Added");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetById(id));

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
        => Ok(await _service.GetByUser(userId));

    // [HttpGet("booking/{bookingId}")]
    // public async Task<IActionResult> GetByBooking(int bookingId)
    //     => Ok(await _service.GetByBooking(bookingId));

    [HttpGet("passport/{passport}")]
    public async Task<IActionResult> GetByPassport(string passport)
        => Ok(await _service.GetByPassport(passport));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PassengerDto dto)
    {
        await _service.Update(id, dto);
        return Ok("Updated");
    }

    [HttpPut("assign-seat")]
    public async Task<IActionResult> AssignSeat(AssignSeatDto dto)
    {
        await _service.AssignSeat(dto);
        return Ok("Seat Assigned");
    }

    [HttpGet("count/{bookingId}")]
    public async Task<IActionResult> Count(int bookingId)
        => Ok(await _service.Count(bookingId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok("Deleted");
    }
}