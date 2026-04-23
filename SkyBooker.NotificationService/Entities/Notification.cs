namespace SkyBooker.NotificationService.Entities;

public class Notification
{
    public int NotificationId { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // BOOKING_CONFIRMED, FLIGHT_DELAY, etc.
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty; // APP, EMAIL, SMS
    public string RelatedBookingId { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
