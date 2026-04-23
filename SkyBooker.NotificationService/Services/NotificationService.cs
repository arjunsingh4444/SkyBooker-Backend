using MailKit.Net.Smtp;
using MimeKit;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkyBooker.NotificationService.Entities;
using SkyBooker.NotificationService.Interfaces;

namespace SkyBooker.NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IConfiguration _configuration;

    public NotificationService(INotificationRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task DeleteNotificationAsync(int notificationId)
    {
        await _repository.DeleteByNotificationIdAsync(notificationId);
    }

    public async Task<IList<Notification>> GetByRecipientAsync(string recipientId)
    {
        return await _repository.FindByRecipientIdAsync(recipientId);
    }

    public async Task<int> GetUnreadCountAsync(string recipientId)
    {
        return await _repository.CountByRecipientIdAndIsReadAsync(recipientId, false);
    }

    public async Task MarkAllReadAsync(string recipientId)
    {
        var unread = await _repository.FindByRecipientIdAndIsReadAsync(recipientId, false);
        foreach (var notif in unread)
        {
            notif.IsRead = true;
            await _repository.UpdateAsync(notif);
        }
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notif = await _repository.GetByIdAsync(notificationId);
        if (notif != null)
        {
            notif.IsRead = true;
            await _repository.UpdateAsync(notif);
        }
    }

    public async Task SendAsync(Notification notification)
    {
        await _repository.CreateAsync(notification);

        if (notification.Channel == "EMAIL")
        {
            await SendEmailAsync(notification.RecipientId, notification.Title, notification.Message);
        }
        else if (notification.Channel == "SMS")
        {
            await SendSMSAsync(notification.RecipientId, notification.Message);
        }
        else if (notification.Channel == "APP")
        {
            // Push notification logic using Firebase Admin SDK goes here
            Console.WriteLine($"[PUSH NOTIFICATION] To {notification.RecipientId}: {notification.Title}");
        }
    }

    public async Task SendBookingConfirmationAsync(string recipientId, string bookingId, string passengerName, string flightDetails)
    {
        // 1. Generate PDF Ticket with QuestPDF
        var pdfBytes = GeneratePdfTicket(bookingId, passengerName, flightDetails);

        // 2. Prepare Email Body
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"<h1>Booking Confirmed</h1><p>Dear {passengerName},</p><p>Your flight <b>{flightDetails}</b> is confirmed. Your e-ticket is attached.</p><p>Thank you for choosing SkyBooker!</p>"
        };

        // 3. Attach PDF
        bodyBuilder.Attachments.Add($"Ticket_{bookingId}.pdf", pdfBytes, new ContentType("application", "pdf"));

        // 4. Send Email via MailKit
        var smtpSettings = _configuration.GetSection("Smtp");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("SkyBooker", smtpSettings["FromEmail"]));
        message.To.Add(new MailboxAddress(passengerName, recipientId)); // Assuming recipientId is the email for this demo
        message.Subject = $"SkyBooker: Booking Confirmation - {bookingId}";
        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(smtpSettings["Host"], int.Parse(smtpSettings["Port"]!), true);
            await client.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }

        // 5. Save Notification Record
        var notif = new Notification
        {
            RecipientId = recipientId,
            Type = "BOOKING_CONFIRMED",
            Title = "Booking Confirmed",
            Message = $"Your booking {bookingId} is confirmed. An email with the ticket has been sent.",
            Channel = "EMAIL",
            RelatedBookingId = bookingId,
            SentAt = DateTime.UtcNow
        };
        await _repository.CreateAsync(notif);
    }

    public async Task SendBulkAsync(IList<string> recipientIds, string type, string title, string message, string channel)
    {
        foreach (var recipient in recipientIds)
        {
            var notif = new Notification
            {
                RecipientId = recipient,
                Type = type,
                Title = title,
                Message = message,
                Channel = channel,
                SentAt = DateTime.UtcNow
            };
            await SendAsync(notif);
        }
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("Smtp");
        
        // Ensure values exist
        if (string.IsNullOrEmpty(smtpSettings["Host"])) return;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("SkyBooker", smtpSettings["FromEmail"]));
        message.To.Add(new MailboxAddress("User", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        try
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(smtpSettings["Host"], int.Parse(smtpSettings["Port"]!), true);
            await client.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    public async Task SendSMSAsync(string toPhone, string message)
    {
        var twilioSettings = _configuration.GetSection("Twilio");
        var accountSid = twilioSettings["AccountSid"];
        var authToken = twilioSettings["AuthToken"];
        var fromPhone = twilioSettings["FromPhoneNumber"];

        if (string.IsNullOrEmpty(accountSid) || accountSid == "your_account_sid")
        {
            Console.WriteLine($"[SMS Mock] To: {toPhone}, Message: {message}");
            return;
        }

        TwilioClient.Init(accountSid, authToken);

        try
        {
            await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(fromPhone),
                to: new Twilio.Types.PhoneNumber(toPhone)
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending SMS: {ex.Message}");
        }
    }

    private byte[] GeneratePdfTicket(string bookingId, string passengerName, string flightDetails)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("SkyBooker E-Ticket")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(10);
                        x.Item().Text($"PNR / Booking ID: {bookingId}").SemiBold();
                        x.Item().Text($"Passenger Name: {passengerName}");
                        x.Item().Text($"Flight Details: {flightDetails}");
                        x.Item().Text($"Date: {DateTime.Now:dd MMM yyyy}");
                        x.Item().Text("Status: CONFIRMED").FontColor(Colors.Green.Medium);
                        
                        x.Item().PaddingTop(20).Text("Thank you for flying with SkyBooker! Have a safe journey.").Italic();
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }
}
