namespace SkyBooker.SeatService.DTOs;

public class AddSeatsDto
{
    public int FlightId { get; set; }
    public int Rows { get; set; }
    public int SeatsPerRow { get; set; }
    public string SeatClass { get; set; } = "Economy";
}