using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Interfaces;

public interface INotificationService
{
    Task SendAsync(Notification notification);
    Task SendBookingConfirmationAsync(string recipientId, string bookingId, string passengerName, string flightDetails);
    Task SendBulkAsync(IList<string> recipientIds, string type, string title, string message, string channel);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllReadAsync(string recipientId);
    Task<IList<Notification>> GetByRecipientAsync(string recipientId);
    Task<int> GetUnreadCountAsync(string recipientId);
    Task DeleteNotificationAsync(int notificationId);
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendSMSAsync(string toPhone, string message);
}
