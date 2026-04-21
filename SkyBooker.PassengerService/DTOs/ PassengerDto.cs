namespace SkyBooker.PassengerService.DTOs;

public class PassengerDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = "";
    public int Age { get; set; }
    public string Gender { get; set; } = "";
    public string PassportNumber { get; set; } = "";
    public string Nationality { get; set; } = "";
}