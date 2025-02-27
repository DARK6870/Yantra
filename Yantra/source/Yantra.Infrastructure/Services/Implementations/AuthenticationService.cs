using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Yantra.Infrastructure.Options;
using Yantra.Infrastructure.Services.Interfaces;

namespace Yantra.Infrastructure.Services.Implementations;

public class AuthenticationService(
    IOptions<JwtOptions> jwtOptions
) : IAuthenticationService
{
    public string GenerateJwtToken(
        string email,
        string role,
        string firstName,
        string lastName
    )
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtOptions.Value.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, firstName + " " + lastName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpireInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}