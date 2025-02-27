namespace Yantra.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    public string GenerateJwtToken(
        string username,
        string email,
        string role
    );
}