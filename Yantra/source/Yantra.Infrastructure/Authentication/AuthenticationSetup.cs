using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Infrastructure.Authentication;

public static class AuthenticationSetup
{
    public static bool EnableSecurity = false;

    public static string AdminAccessPolicy = "AdminAccessPolicy";
    public static string ManagerAccessPolicy = "ManagerAccessPolicy";
    public static string CourierAccessPolicy = "CourierAccessPolicy";

    private static readonly string[] AdminAccessPolicyRoles = [Role.Admin.ToString()];
    private static readonly string[] ManagerAccessPolicyRoles = [Role.Admin.ToString(), Role.Manager.ToString()];
    private static readonly string[] CourierAccessPolicyRoles = [Role.Courier.ToString()];

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        EnableSecurity = configuration.GetValue<bool>("AuthenticationOptions:EnableSecurity");

        services
            .AddAuthorizationBuilder()
            .AddPolicy(AdminAccessPolicy, policy => policy.RequireRole(AdminAccessPolicyRoles))
            .AddPolicy(ManagerAccessPolicy, policy => policy.RequireRole(ManagerAccessPolicyRoles))
            .AddPolicy(CourierAccessPolicy, policy => policy.RequireRole(CourierAccessPolicyRoles))
            ;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration.GetValue<string>("AuthenticationOptions:JwtOptions:SecretKey")
                            ?? throw new NullReferenceException("Security key is missing.")
                        )
                    )
                };
            });

        return services;
    }
}