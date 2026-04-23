using Microsoft.EntityFrameworkCore;
using SkyBooker.NotificationService.Data;
using SkyBooker.NotificationService.Entities;
using SkyBooker.NotificationService.Interfaces;

namespace SkyBooker.NotificationService.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountByRecipientIdAndIsReadAsync(string recipientId, bool isRead)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId && n.IsRead == isRead)
            .CountAsync();
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task DeleteByNotificationIdAsync(int notificationId)
    {
        var notif = await _context.Notifications.FindAsync(notificationId);
        if (notif != null)
        {
            _context.Notifications.Remove(notif);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IList<Notification>> FindByRecipientIdAndIsReadAsync(string recipientId, bool isRead)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId && n.IsRead == isRead)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<IList<Notification>> FindByRecipientIdAsync(string recipientId)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<IList<Notification>> FindByRelatedBookingIdAsync(string bookingId)
    {
        return await _context.Notifications
            .Where(n => n.RelatedBookingId == bookingId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<IList<Notification>> FindByTypeAsync(string type)
    {
        return await _context.Notifications
            .Where(n => n.Type == type)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int notificationId)
    {
        return await _context.Notifications.FindAsync(notificationId);
    }

    public async Task UpdateAsync(Notification notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }
}
