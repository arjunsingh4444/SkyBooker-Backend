using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Interfaces;

namespace SkyBooker.BookingService.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto)
        => Ok(await _service.CreateBooking(dto));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _service.GetBookingById(id));

    [HttpGet("pnr/{pnr}")]
    public async Task<IActionResult> GetByPnr(string pnr)
        => Ok(await _service.GetBookingByPnr(pnr));

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
        => Ok(await _service.GetBookingsByUser(userId));

    [HttpGet("flight/{flightId}")]
    public async Task<IActionResult> GetByFlight(int flightId)
        => Ok(await _service.GetBookingsByFlight(flightId));

    [HttpGet("upcoming/{userId}")]
    public async Task<IActionResult> Upcoming(int userId)
        => Ok(await _service.GetUpcomingBookings(userId));

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelBooking(id);
        return Ok("Cancelled");
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(Guid id, string status)
    {
        await _service.UpdateStatus(id, status);
        return Ok();
    }

    [HttpGet("fare")]
    public async Task<IActionResult> Fare(decimal baseFare, decimal taxes, decimal ancillary)
        => Ok(await _service.CalculateFare(baseFare, taxes, ancillary));

    [HttpPost("addon")]
    public async Task<IActionResult> AddOn(AddOnDto dto)
    {
        await _service.AddAddOn(dto);
        return Ok();
    }
}