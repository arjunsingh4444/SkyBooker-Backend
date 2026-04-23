using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.NotificationService.Interfaces;

namespace SkyBooker.NotificationService.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("recipient/{recipientId}")]
    public async Task<IActionResult> GetByRecipient(string recipientId)
    {
        var notifications = await _notificationService.GetByRecipientAsync(recipientId);
        return Ok(notifications);
    }

    [HttpGet("recipient/{recipientId}/unreadCount")]
    public async Task<IActionResult> GetUnreadCount(string recipientId)
    {
        var count = await _notificationService.GetUnreadCountAsync(recipientId);
        return Ok(new { Count = count });
    }

    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        await _notificationService.MarkAsReadAsync(notificationId);
        return NoContent();
    }

    [HttpPut("recipient/{recipientId}/readAll")]
    public async Task<IActionResult> MarkAllRead(string recipientId)
    {
        await _notificationService.MarkAllReadAsync(recipientId);
        return NoContent();
    }

    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        await _notificationService.DeleteNotificationAsync(notificationId);
        return NoContent();
    }

    // Usually restricted to internal services, using an InternalService policy or role
    [HttpPost("bulk")]
    [Authorize(Roles = "Admin,AirlineStaff,Internal")]
    public async Task<IActionResult> SendBulk([FromBody] BulkNotificationRequest request)
    {
        await _notificationService.SendBulkAsync(request.RecipientIds, request.Type, request.Title, request.Message, request.Channel);
        return Ok();
    }
}

public class BulkNotificationRequest
{
    public IList<string> RecipientIds { get; set; } = new List<string>();
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
}
