namespace SkyBooker.FlightService.DTOs;

public class FlightDto
{
    public int FlightId { get; set; }
    public string FlightNumber { get; set; } = "";
    public int AirlineId { get; set; }
    public string OriginAirportCode { get; set; } = "";
    public string DestinationAirportCode { get; set; } = "";
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = "";
    public string AircraftType { get; set; } = "";
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal BasePrice { get; set; }
}