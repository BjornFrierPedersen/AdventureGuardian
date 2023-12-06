using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Worlds;
using AdventureGuardian.Test.Stubs;
using FluentAssertions;
using TinyHeroesRp.Services.Domain;
using Xunit;

namespace AdventureGuardian.Test;

public class WorldServiceTest
{
    private readonly WorldService _worldService = new(new TestOpenAiCommunicatorService());


    [Theory]
    [InlineData(World.WorldType.SciFi, new[] { 8, 9, 11 }, true, false)]
    [InlineData(World.WorldType.Fantasy, new[] { 12, 13, 15 }, false, false)]
    [InlineData(World.WorldType.Realism, new[] { 16, 17, 122 }, true, true)]
    public async Task Generating_world_for_individuals_from_same_age_groups_should_correctly_set_explicit_content(
        World.WorldType worldType, int[] playersByAge, bool displayExplicitContent, bool expectedOutput)
    {
        // Arrange
        var worldService = new WorldService(new TestOpenAiCommunicatorService());
        // Act
        var generatedWorld = await worldService
            .GenerateWorldAsync($"test_{worldType}", playersByAge, worldType, displayExplicitContent);
        // Assert
        generatedWorld.ExplicitContent.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(World.WorldType.SciFi, new[] { 7, 8 }, false, false)]
    [InlineData(World.WorldType.Fantasy, new[] { 11, 12 }, true, false)]
    [InlineData(World.WorldType.Realism, new[] { 15, 16 }, false, false)]
    public async Task Generating_world_for_individuals_from_different_age_groups_should_correctly_set_explicit_content(
        World.WorldType worldType, int[] playersByAge, bool displayExplicitContent, bool expectedOutput)
    {
        // Arrange
        var worldService = new WorldService(new TestOpenAiCommunicatorService());
        // Act
        var generatedWorld = await worldService
            .GenerateWorldAsync($"test_{worldType}", playersByAge, worldType, displayExplicitContent);
        // Assert
        generatedWorld.ExplicitContent.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(World.WorldType.SciFi, new[] { 3, 4 })]
    [InlineData(World.WorldType.Fantasy, new[] { 122, 123 })]
    [InlineData(World.WorldType.Realism, new[] { 1, 123 })]
    public void Generating_world_for_individuals_with_age_outside_the_allowed_age_groups_should_throw_an_exception(
        World.WorldType worldType, int[] playersByAge)
    {
        // Arrange
        var worldService = new WorldService(new TestOpenAiCommunicatorService());
        // Act
        var worldGenerationAction = () =>
        {
            var _ = worldService
                .GenerateWorldAsync($"test_{worldType}", playersByAge, worldType, false).Result;
        };
        // Assert
        worldGenerationAction.Should().Throw<ArgumentException>();
    }
}