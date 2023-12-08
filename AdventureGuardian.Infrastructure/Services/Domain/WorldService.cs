using AdventureGuardian.Models.Models.Worlds;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class WorldService
{
    private readonly IOpenAiCommunicatorService _openAiCommunicatorService;

    public WorldService(IOpenAiCommunicatorService openAiCommunicatorService)
    {
        _openAiCommunicatorService = openAiCommunicatorService;
    }

    public async Task<World> GenerateWorldAsync(string name, int[] playersByAge, World.WorldType worldType,
        bool displayExplicitContent, string[]? keywords = null)
    {
        if (playersByAge.Any(age => age < 4))
            throw new ArgumentException("Players must be at least 4 years old to play this game");
        if (playersByAge.Any(age => age > 122))
            throw new ArgumentException("Players must be no more than 122 years old to play this game");

        var shouldDisplayExplicitContent = ShouldDisplayExplicitContent(playersByAge, displayExplicitContent);
        var world = CreateWorld(worldType, name, shouldDisplayExplicitContent);
        return await GenerateWorldDescriptionAsync(world, playersByAge, keywords);
    }

    public async Task<World> GenerateWorldDescriptionAsync(World world, int[] playersByAge, string[]? keywords = null)
    {
        var worldDescription = await _openAiCommunicatorService.SendRequestAsync(world.Prompt(playersByAge, keywords));
        world.Description = worldDescription;
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
}