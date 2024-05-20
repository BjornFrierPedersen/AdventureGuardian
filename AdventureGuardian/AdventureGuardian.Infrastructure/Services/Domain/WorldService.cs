using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models;
using AdventureGuardian.Models.Models.Domain.Worlds;
using MessageGateway;
using MessageGateway.Events;
using Shared.Enums;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class WorldService
{
    private readonly RequestProducer _producer;
    private readonly CampaignRepository _campaignRepository;
    public WorldService(CampaignRepository campaignRepository, RequestProducer producer)
    {
        _campaignRepository = campaignRepository;
        _producer = producer;
    }

    public World GenerateWorld(string name, int[] playersByAge, World.WorldType worldType,
        bool displayExplicitContent, string[]? worldKeywords = null)
    {
        if (playersByAge.Any(age => age < 4))
            throw new ArgumentException("Players must be at least 4 years old to play this game");
        if (playersByAge.Any(age => age > 122))
            throw new ArgumentException("Players must be no more than 122 years old to play this game");

        var shouldDisplayExplicitContent = ShouldDisplayExplicitContent(playersByAge, displayExplicitContent);
        var world = CreateWorld(worldType, name, shouldDisplayExplicitContent);
        return GenerateWorldDescription(world, playersByAge, worldKeywords);
    }

    public World GenerateWorldDescription(World world, int[] playersByAge, string[]? keywords = null)
    {
            _producer.Publish(
            new RequestEvent(world.ExternalId, EntityType.World, world.Prompt(playersByAge, keywords)));
        world.Description = KnownStringVariables.FetchingGeneratedResponse;
        return world;
    }

    private World CreateWorld(World.WorldType worldType, string name, bool shouldDisplayExplicitContent)
    {
        return worldType switch
        {
            World.WorldType.Fantasy => new FantasyWorld(shouldDisplayExplicitContent) { Name = name },
            World.WorldType.SciFi => new SciFiWorld(shouldDisplayExplicitContent) { Name = name },
            World.WorldType.Realism => new RealismWorld(shouldDisplayExplicitContent) { Name = name },
            _ => throw new ArgumentOutOfRangeException(nameof(worldType), worldType, null)
        };
    }

    private bool ShouldDisplayExplicitContent(IEnumerable<int> playersByAge, bool displayExplicitContent)
    {
        if (!displayExplicitContent) return false;

        return displayExplicitContent && !playersByAge.Any(age => age < 16);
    }

    public async Task<World> GetByIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await _campaignRepository.GetWorldByIdAsync(externalId, cancellationToken);
    }
    
    public async Task UpdateAsync(World world, CancellationToken cancellationToken)
    {
        await _campaignRepository.UpdateWorldAsync(world, cancellationToken);
    }
}