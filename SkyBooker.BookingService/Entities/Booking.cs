using System.ComponentModel.DataAnnotations;

namespace SkyBooker.BookingService.Entities;

public class Booking
{
    [Key]
    public Guid BookingId { get; set; } = Guid.NewGuid();

    public int UserId { get; set; }
    public int FlightId { get; set; }

    public string PnrCode { get; set; } = "";

    public string TripType { get; set; } = "ONE_WAY";
    public string Status { get; set; } = "CONFIRMED";

    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalFare { get; set; }

    public string MealPreference { get; set; } = "";
    public int LuggageKg { get; set; }

    public string ContactEmail { get; set; } = "";
    public string ContactPhone { get; set; } = "";

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public string PaymentId { get; set; } = "";
}