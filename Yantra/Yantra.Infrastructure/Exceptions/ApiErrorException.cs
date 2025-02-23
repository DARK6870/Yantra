using System.Net;

namespace Yantra.Infrastructure.Exceptions;

public class ApiErrorException : Exception
{
    public List<string> Errors { get; }
    public HttpStatusCode StatusCode { get; }
    

    public ApiErrorException(List<string> errors, HttpStatusCode statusCode)
        :base(string.Join("\n", errors))
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        StatusCode = statusCode;
    }
    
    public ApiErrorException(string error, HttpStatusCode statusCode)
        : base(error)
    {
        Errors = [error ?? throw new ArgumentNullException(nameof(error))];
        StatusCode = statusCode;
    }
}