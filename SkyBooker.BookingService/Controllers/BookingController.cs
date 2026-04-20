using Microsoft.AspNetCore.Mvc;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Interfaces;

namespace SkyBooker.BookingService.Controllers;

[ApiController]
[Route("api/bookings")]
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

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _service.CancelBooking(id);
        return Ok("Cancelled");
    }

    [HttpGet("fare")]
    public async Task<IActionResult> Fare(int seats, int luggage)
        => Ok(await _service.CalculateFare(seats, luggage));
}