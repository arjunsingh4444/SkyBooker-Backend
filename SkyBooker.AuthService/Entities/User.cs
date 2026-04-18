using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.AuthService.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Column]
    public string FullName { get; set; } = string.Empty;

    [Column]
    public string Email { get; set; } = string.Empty;

    [Column]
    public string PasswordHash { get; set; } = string.Empty;

    [Column]
    public string Phone { get; set; } = string.Empty;

    [Column]
    public string Role { get; set; } = "PASSENGER";

    [Column]
    public string Provider { get; set; } = "LOCAL";

    [Column]
    public bool IsActive { get; set; } = true;

    [Column]
    public string PassportNumber { get; set; } = string.Empty;

    [Column]
    public string Nationality { get; set; } = string.Empty;

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}