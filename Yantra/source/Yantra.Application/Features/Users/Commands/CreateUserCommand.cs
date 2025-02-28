using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Models;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Application.Features.Users.Commands;

public record CreateUserCommand(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    Role Role
) : IRequest<bool>;

public class CreateUserCommandHandler(
    IUsersRepository usersRepository,
    INotificationService notificationService
) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserEntity
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        };
        
        await usersRepository.InsertOneAsync(user, cancellationToken);

        var emailMessage = new MessageBuilder
        {
            Title = "Your account has been successfully created",
            FullName = user.FirstName + " " + user.LastName,
            Message = "Your account has been successfully created! To log in to Yantra, please follow the link below and change your password.",
            ActionUrl = "https://google.com",
            ActionText = "to log in to Yantra."
        };
        
        await notificationService.SendEmailNotification(
            user.Email,
            NotificationTypes.AccountRegistration,
            emailMessage
        );
            
        return true;
    }
}