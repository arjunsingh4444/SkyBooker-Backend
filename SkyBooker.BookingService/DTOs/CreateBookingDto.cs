namespace SkyBooker.BookingService.DTOs;

public class CreateBookingDto
{
    public int UserId { get; set; }
    public int FlightId { get; set; }

    public string TripType { get; set; } = "ONE_WAY";

    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }

    public string MealPreference { get; set; } = "";
    public int LuggageKg { get; set; }

    public string ContactEmail { get; set; } = "";
    public string ContactPhone { get; set; } = "";
}