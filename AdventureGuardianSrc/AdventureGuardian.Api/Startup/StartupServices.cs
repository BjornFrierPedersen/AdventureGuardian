using System.Text.Json.Serialization;
using AdventureGuardian.Api.Helpers;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Infrastructure.Services.Domain;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace AdventureGuardian.Api.Startup;

public static partial class Startup
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
        builder.Services.AddTransient<IMessagePublisher, RabbitMqPublisher>();
        builder.Services.AddTransient<IMessageSubscriber, RabbitMqSubscriber<OpenAiResponseRoute>>();
        builder.Services.AddHttpContextAccessor();
        // DbContext
        builder.Services.AddDbContext<AdventureGuardianDbContext>();
        // Repositories
        builder.Services.AddTransient<CampaignRepository>();
        builder.Services.AddTransient<CharacterRepository>();
        builder.Services.AddTransient<WorldRepository>();
        // Services
        builder.Services.AddTransient<IClaimsHandlerService, ClaimsHandlerService>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddTransient<WorldService>();
        builder.Services.AddTransient<CampaignService>();
        builder.Services.AddTransient<CharacterService>();
        builder.Services.AddTransient<EncounterService>();
        builder.Services.AddTransient<WorldService>();
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
    

}