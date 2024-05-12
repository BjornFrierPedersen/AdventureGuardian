using AdventureGuardian.Api.Helpers;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models;
using AdventureGuardian.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AdventureGuardian.Api.Startup;

public static partial class Startup
{
    
    public static void MapCampaignEndpoints(this WebApplication app)
    {
        app.MapGet("/campaign/get", async (CampaignService campaignService, CancellationToken cancellationToken) 
            => await campaignService.Campaigns(cancellationToken)).WithDefaultAuthorization();
        app.MapPost("/campaign/create", async ([FromBody]CreateCampaignDto createCampaignDto, CampaignService campaignService, CancellationToken cancellationToken) 
            => await campaignService.CreateCampaignAsync(createCampaignDto, cancellationToken)).WithDefaultAuthorization();
    }
    
    public static void MapHelpEndpoints(this WebApplication app)
    {
       app.MapGet("/help/token/get",
            async ([FromQuery] string username, [FromQuery] string password, TokenService tokenService,
                    CancellationToken cancellationToken) =>
                await tokenService.GetDevelopmentToken(username, password, cancellationToken));
    }
    
    private static void WithDefaultAuthorization(this RouteHandlerBuilder routeHandlerBuilder)
    {
        routeHandlerBuilder.RequireAuthorization(KnownPolicies.UserId, KnownPolicies.BaseRole);
    }
}