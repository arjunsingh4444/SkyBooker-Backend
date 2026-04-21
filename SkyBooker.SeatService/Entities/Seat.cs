using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SkyBooker.SeatService.Entities;

public class Seat
{
    [Key]
    public int SeatId { get; set; }

    public int FlightId { get; set; }

    public string SeatNumber { get; set; } = ""; // e.g., 12A
    public string SeatClass { get; set; } = "Economy"; // Economy/Business/First

    public int Row { get; set; }
    public string Column { get; set; } = "";

    public bool IsWindow { get; set; }
    public bool IsAisle { get; set; }
    public bool HasExtraLegroom { get; set; }

    [ConcurrencyCheck] // optimistic concurrency
    public string Status { get; set; } = "AVAILABLE"; // AVAILABLE/HELD/CONFIRMED/BLOCKED

    public decimal PriceMultiplier { get; set; } = 1.0m;

    public DateTime? HeldSince { get; set; }
}