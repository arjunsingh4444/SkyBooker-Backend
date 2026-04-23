namespace SkyBooker.AuthService.DTOs;

public class UpdateProfileDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
}