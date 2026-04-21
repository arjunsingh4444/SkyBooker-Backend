namespace SkyBooker.BookingService.DTOs;

public class AddOnDto
{
    public Guid BookingId { get; set; }
    public int ExtraLuggageKg { get; set; }
    public string Meal { get; set; } = "";
}