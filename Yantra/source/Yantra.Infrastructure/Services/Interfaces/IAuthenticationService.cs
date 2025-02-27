namespace Yantra.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    public string GenerateJwtToken(
        string email,
        string role,
        string firstName,
        string lastName
    );
}