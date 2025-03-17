using System.Net;
using System.Net.Mail;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Models;
using Yantra.Notifications.Options;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Notifications.Services.Implementations;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    IOptions<NotificationOptions> notificationOptions,
    ILogger<NotificationService> logger,
    IWebHostEnvironment webHostEnvironment
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    private readonly NotificationOptions _notificationOptions = notificationOptions.Value;

    public async Task SendEmailNotification(
        string emailTo,
        string subject,
        MessageBuilder message
    )
    {
        var body = await GetEmailTemplateAsync(MessageType.Notification, message.FormattedMessage);

        await SendEmailAsync(emailTo, subject, body);
    }

    public async Task SendEmailToSupport(SupportMessageBuilder message)
    {
        var body = await GetEmailTemplateAsync(MessageType.SupportRequest, message.FormattedMessage);
        
        await SendEmailAsync(_notificationOptions.SupportEmail, "Support Request", body);
    }


    private async Task SendEmailAsync(
        string emailTo,
        string subject,
        string body
    )
    {
        using var smtpClient = new SmtpClient(_smtpOptions.SmtpServer)
        {
            Port = _smtpOptions.SmtpPort,
            Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpOptions.SmtpEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(emailTo);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);

            logger.LogInformation(
                "Email with subject '{subject}' was successfully sent to '{emailTo}'.",
                subject,
                emailTo
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to send email to '{emailTo}', subject '{subject}'",
                emailTo,
                subject
            );
            
            throw;
        }
    }


    private async Task<string> GetEmailTemplateAsync(
        MessageType messageType,
        string content
    )
    {
        var rootPath = webHostEnvironment.ContentRootPath;
        
        var templatesPath = Path.Combine(rootPath, "..", "Yantra.Notifications", "Templates");
        var filePath = Path.Combine(templatesPath, messageType + ".html");
        
        var htmlTemplate = await File.ReadAllTextAsync(filePath);
        var finalHtml = htmlTemplate.Replace("{Content}", content);

        return finalHtml;
    }
}