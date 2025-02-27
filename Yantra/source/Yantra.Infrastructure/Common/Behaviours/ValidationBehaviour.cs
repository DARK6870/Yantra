using FluentValidation;
using MediatR;

namespace Yantra.Infrastructure.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validator = validators.FirstOrDefault();
        
        if (validator != null)
            await validator.ValidateAndThrowAsync(request, cancellationToken);

        var response = await next();
        return response;
    }
}