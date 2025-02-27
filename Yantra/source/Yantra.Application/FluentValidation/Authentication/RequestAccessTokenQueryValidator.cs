using FluentValidation;
using Yantra.Application.Features.Authentication.Queries;

namespace Yantra.Application.FluentValidation.Authentication;

public class RequestAccessTokenQueryValidator : AbstractValidator<RequestAccessTokenQuery>
{
    public RequestAccessTokenQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}