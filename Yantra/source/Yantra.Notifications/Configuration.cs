using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Notifications.Options;
using Yantra.Notifications.Services.Implementations;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Notifications;

public static class Configuration
{
    public static IServiceCollection AddNotificationService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
        services.Configure<NotificationOptions>(configuration.GetSection(nameof(NotificationOptions)));
        
        services.AddSingleton<INotificationService, NotificationService>();
        
        return services;
    }
}