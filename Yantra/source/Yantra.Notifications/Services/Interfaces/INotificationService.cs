using Yantra.Notifications.Builders;

namespace Yantra.Notifications.Services.Interfaces;

public interface INotificationService
{
    Task SendEmailNotification(string emailTo, string subject, MessageBuilder message);
}