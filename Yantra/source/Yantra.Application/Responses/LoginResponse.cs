namespace Yantra.Application.Responses;

public record LoginResponse(string RefreshToke, string AccessToken);