using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Options;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Notifications.Services.Implementations;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    ILogger<NotificationService> logger
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendEmailNotification(
        string emailTo,
        string subject,
        MessageBuilder message
    )
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = _smtpOptions.SmtpPort,
            Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpOptions.SmtpEmail),
            Subject = subject,
            Body = await GetEmailTemplateAsync(message.FormattedMessage),
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
                "Failed to send notification to '{emailTo}', subject '{subject}'",
                emailTo,
                subject
            );
            
            throw;
        }
    }

    private async Task<string> GetEmailTemplateAsync(string content)
    {
        var filePath = "../Yantra.Notifications/Templates/NotificationTemplate.html";

        var htmlTemplate = await File.ReadAllTextAsync(filePath);
        var finalHtml = htmlTemplate.Replace("{Content}", content);

        return finalHtml;
    }
}