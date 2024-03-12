using System.Text.Json.Serialization;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Infrastructure.Services.Domain;

namespace AdventureGuardian.Api;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
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
            => await campaignService.Campaigns(cancellationToken));
    }
}