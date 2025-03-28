using System.Text;

namespace Yantra.Notifications.Builders;

public class SupportMessageBuilder
{
    public string? Title { get; set; }
    public string? FullName { get; set; }
    public required string Message { get; set; }

    public required string ReplyEmail { get; set; }
    
    public string FormattedMessage
    {
        get
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append($"<h2>{Title}</h2>");
            stringBuilder.Append($"<p>Support request from <strong>{FullName}</strong>!</p>");
            stringBuilder.Append($"<p>{Message}</p>");
            stringBuilder.Append($"<p><small>Contact Info: {ReplyEmail}</small></p>");

            return stringBuilder.ToString();
        }
    }
}