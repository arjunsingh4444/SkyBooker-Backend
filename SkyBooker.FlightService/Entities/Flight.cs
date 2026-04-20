using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkyBooker.FlightService.Entities;

[Index(nameof(FlightNumber), IsUnique = true)]
[Index(nameof(OriginAirportCode), nameof(DestinationAirportCode), nameof(DepartureTime))]
public class Flight
{
    [Key]
    public int FlightId { get; set; }

    [Column]
    public string FlightNumber { get; set; } = "";

    [Column]
    public int AirlineId { get; set; }

    [Column]
    public string OriginAirportCode { get; set; } = "";

    [Column]
    public string DestinationAirportCode { get; set; } = "";

    [Column]
    public DateTime DepartureTime { get; set; }

    [Column]
    public DateTime ArrivalTime { get; set; }

    [Column]
    public int DurationMinutes { get; set; }

    [Column]
    public string Status { get; set; } = "Scheduled";

    [Column]
    public string AircraftType { get; set; } = "";

    [Column]
    public int TotalSeats { get; set; }

    [Column]
    public int AvailableSeats { get; set; }

    [Column]
    public decimal BasePrice { get; set; }
}