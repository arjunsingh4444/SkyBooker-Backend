namespace SkyBooker.BookingService.DTOs;

public class CreateBookingDto
{
    public int UserId { get; set; }
    public int FlightId { get; set; }
    public int Seats { get; set; }

    public string ContactEmail { get; set; } = "";
    public string ContactPhone { get; set; } = "";

    public int LuggageKg { get; set; }
}