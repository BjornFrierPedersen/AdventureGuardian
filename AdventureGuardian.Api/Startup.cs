using System.Security.Authentication;
using System.Text.Json.Serialization;
using AdventureGuardian.Api.Helpers;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AdventureGuardian.Api;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        // DbContext
        builder.Services.AddDbContext<AdventureGuardianDbContext>();
        // Repositories
        builder.Services.AddTransient<CampaignRepository>();
        builder.Services.AddTransient<CharacterRepository>();
        builder.Services.AddTransient<WorldRepository>();
        // Services
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddTransient<WorldService>();
        builder.Services.AddTransient<CampaignService>();
        builder.Services.AddTransient<CharacterService>();
        builder.Services.AddTransient<EncounterService>();
        builder.Services.AddTransient<WorldService>();
        builder.Services.AddTransient<IOpenAiCommunicatorService, OpenAiCommunicatorService>();
    }
    
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/campaigns", async (CampaignService campaignService, CancellationToken cancellationToken) 
            => await campaignService.Campaigns(cancellationToken)).RequireAuthorization();
        app.MapGet("/token",
            async ([FromQuery] string username, [FromQuery] string password, TokenService tokenService,
                    CancellationToken cancellationToken) =>
                await tokenService.GetDevelopmentToken(username, password, cancellationToken));
    }
    
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
        services.AddAuthorization();
    }
    
    public static void AddCustomSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo  
            {  
                Version = "v1",
                Title = "AdventureGuardian API",
                Description = "An API for AdventureGuardian."
            });
            swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer {token}' here",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            });
            swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { } 
                }
            });
        });
    }
    
    public static T ConfigureOption<T>(this WebApplicationBuilder builder, string section) where T : class, new()
    {
        var optionsSection = builder.Configuration.GetSection(section);
        var options = new T();
        optionsSection.Bind(options);
        builder.Services.Configure<T>(optionsSection);
        return options;
    }
}