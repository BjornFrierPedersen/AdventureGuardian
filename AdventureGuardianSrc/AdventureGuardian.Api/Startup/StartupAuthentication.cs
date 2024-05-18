using System.Security.Authentication;
using System.Security.Claims;
using AdventureGuardian.Models;
using AdventureGuardian.Models.Models.Authentication;
using AdventureGuardian.Models.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AdventureGuardian.Api.Startup;

public static partial class Startup
{
    public static void AddOpenIdAuthentication(this IServiceCollection services, IdpOptions idpOptions, bool isLive)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Authority = idpOptions.Authority;
                jwtBearerOptions.Audience = idpOptions.Audience;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = idpOptions.Authority,
                    ValidateAudience = isLive,
                    ValidateIssuer = isLive,
                    ValidateIssuerSigningKey = isLive,
                    ValidateLifetime = isLive,
                    RequireSignedTokens = isLive,
                };
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                        throw new AuthenticationException(
                            $"Access - OpenID Authentication failed:\n{context.Exception} for user with IP-address: {context.HttpContext.Connection.RemoteIpAddress}")
                };
                jwtBearerOptions.RequireHttpsMetadata = isLive;
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(KnownPolicies.UserId, policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
            // Roles policies
            options.AddPolicy(KnownPolicies.BaseRole, policy => policy.RequireRole(Roles.AdventureGuardian));
            options.AddPolicy(KnownPolicies.WyrmlingRole, policy => policy.RequireRole(Roles.AdventureGuardianWyrmling));
            options.AddPolicy(KnownPolicies.DrakeRole, policy => policy.RequireRole(Roles.AdventureGuardianDrake));
            options.AddPolicy(KnownPolicies.DragonRole, policy => policy.RequireRole(Roles.AdventureGuardianDragon));
        });
    }
}