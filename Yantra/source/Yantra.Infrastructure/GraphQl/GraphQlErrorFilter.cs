using FluentValidation;
using Yantra.Infrastructure.Common.Exceptions;

namespace Yantra.Infrastructure.GraphQl;

public class GraphQlErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return GetErrorResponse(error);
    }

    private static IError GetErrorResponse(IError error)
    {
        var errorBuilder = ErrorBuilder
            .FromError(error)
            .ClearExtensions()
            .ClearLocations();
        
        switch (error.Exception)
        {
            case ApiErrorException apiErrorException:
                HandleApiErrorException(errorBuilder, apiErrorException);
                break;
            case ValidationException validationException:
                HandleValidationException(errorBuilder, validationException);
                break;
        }
        
        return errorBuilder.Build();
    }

    private static void HandleApiErrorException(IErrorBuilder errorBuilder, ApiErrorException apiErrorException)
    {
        errorBuilder
            .SetMessage(apiErrorException.Message)
            .SetCode(((int)apiErrorException.StatusCode)
                .ToString());
    }
    
    private static void HandleValidationException(IErrorBuilder errorBuilder, ValidationException validationException)
    {
        errorBuilder
            .SetMessage("Invalid request")
            .SetCode("INVALID_INPUT")
            .SetExtension("validationErrors", validationException.Errors);
    }
}