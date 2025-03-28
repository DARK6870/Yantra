using MediatR;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Application.Features.Support.Commands;

public record CreateSupportRequestCommand(
    string Title,
    string Message,
    string FullName,
    string ReplyEmail
) : IRequest<bool>;

public class CreateSupportRequestCommandHandler(
    INotificationService notificationService
) : IRequestHandler<CreateSupportRequestCommand, bool>
{
    public async Task<bool> Handle(CreateSupportRequestCommand request, CancellationToken cancellationToken)
    {
        var email = new SupportMessageBuilder()
        {
            Title = request.Title,
            Message = request.Message,
            FullName = request.FullName,
            ReplyEmail = request.ReplyEmail,
        };

        await notificationService.SendEmailToSupport(email);

        return true;
    }
}