namespace Yantra.Notifications.Options;

public class SmtpOptions
{
    public required string SmtpServer { get; set; }
    public required int SmtpPort { get; set; }
    public required string SmtpEmail { get; set; }
    public required string SmtpPassword { get; set; }
}