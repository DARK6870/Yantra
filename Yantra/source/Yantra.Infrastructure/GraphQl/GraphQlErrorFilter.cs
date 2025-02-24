using FluentValidation;
using Microsoft.Extensions.Logging;
using Yantra.Infrastructure.Exceptions;

namespace Yantra.Infrastructure.GraphQl;

public class GraphQlErrorFilter(ILogger<GraphQlErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        logger.LogError(
            "An error occured while processing request. Message: {message}",
            error.Exception?.InnerException?.Message
            ?? error.Exception?.Message
            ?? error.Message
        );
        
        return GetErrorResponse(error);
    }

    private IError GetErrorResponse(IError error)
    {
        IErrorBuilder errorBuilder = ErrorBuilder
            .FromError(error)
            .ClearExtensions()
            .ClearLocations();
        
        if (error.Exception is ApiErrorException apiErrorException)
        {
            HandleApiErrorException(errorBuilder, apiErrorException);
        }
        else if (error.Exception is ValidationException validationException)
        {
            HandleValidationException(errorBuilder, validationException);
        }
        else
        {
            errorBuilder
                .SetMessage("An error occured while processing request.")
                .SetCode("500");
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
            .SetExtension("errors", validationException.Errors);
    }
}