using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Interfaces;

public interface INotificationRepository
{
    Task<IList<Notification>> FindByRecipientIdAsync(string recipientId);
    Task<IList<Notification>> FindByRecipientIdAndIsReadAsync(string recipientId, bool isRead);
    Task<int> CountByRecipientIdAndIsReadAsync(string recipientId, bool isRead);
    Task<IList<Notification>> FindByTypeAsync(string type);
    Task<IList<Notification>> FindByRelatedBookingIdAsync(string bookingId);
    Task DeleteByNotificationIdAsync(int notificationId);
    Task<Notification> CreateAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task<Notification?> GetByIdAsync(int notificationId);
}
