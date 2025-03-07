using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Yantra.Infrastructure.Authentication;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Infrastructure.Services.Implementations;

public class UserContext(
    IHttpContextAccessor httpContextAccessor
) : IUserContext
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User
                                             ?? throw new ApiErrorException("You are not logged in.", HttpStatusCode.Unauthorized);

    public string UserName => _user.Identity?.Name ?? "Unknown";

    public Role Role
    {
        get
        {
            if (!AuthenticationSetup.EnableSecurity)
                return Role.Admin;
            
            var roleValue = _user.FindFirst(ClaimTypes.Role)?.Value;

            if (roleValue == null || !Enum.TryParse<Role>(roleValue, out var role))
            {
                throw new NullReferenceException("Role not found or invalid.");
            }

            return role;
        }
    }
}