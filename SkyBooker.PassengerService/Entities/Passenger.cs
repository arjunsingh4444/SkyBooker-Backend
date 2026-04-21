using System.ComponentModel.DataAnnotations;

namespace SkyBooker.PassengerService.Entities;

public class Passenger
{
    [Key]
    public int PassengerId { get; set; }

    public int UserId { get; set; }
    public string FullName { get; set; } = "";
    public int Age { get; set; }
    public string Gender { get; set; } = "";

    public string PassportNumber { get; set; } = "";
    public string Nationality { get; set; } = "";

    public int? BookingId { get; set; }
    public int? SeatId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}