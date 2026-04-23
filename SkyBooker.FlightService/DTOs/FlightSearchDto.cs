namespace SkyBooker.FlightService.DTOs;

public class FlightSearchDto
{
    public string Origin { get; set; } = "";
    public string Destination { get; set; } = "";
    public DateTime Date { get; set; }
}