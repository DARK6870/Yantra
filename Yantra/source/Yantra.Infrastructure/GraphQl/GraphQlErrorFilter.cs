using FluentValidation;
using Yantra.Infrastructure.Exceptions;

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
            default:
                errorBuilder
                    .SetMessage("An error occured while processing request.")
                    .SetCode("500");
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
            .SetMessage("Invalid data input")
            .SetCode("INVALID_INPUT")
            .SetExtension("validationErrors", validationException.Errors);
    }
}