using FluentValidation;
using Yantra.Application.Features.Authentication.Queries;

namespace Yantra.Application.FluentValidation.Authentication;

public class RequestAccessTokenQueryValidator : AbstractValidator<RequestAccessTokenQuery>
{
    public RequestAccessTokenQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty")
            .EmailAddress()
                .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password can not be empty");
    }
}