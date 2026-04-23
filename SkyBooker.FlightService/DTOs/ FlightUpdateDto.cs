namespace SkyBooker.FlightService.DTOs;

public class FlightUpdateDto
{
    public int FlightId { get; set; }
    public string AircraftType { get; set; } = "";
    public int TotalSeats { get; set; }
    public decimal BasePrice { get; set; }
}