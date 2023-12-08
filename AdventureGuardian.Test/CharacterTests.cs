using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Models.ClassModels;
using AdventureGuardian.Models.Models.RaceModels;
using AdventureGuardian.Test.Database_Handling;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AdventureGuardian.Test;

[Collection(TestVariables.DatabaseCollection)]
public class CharacterTests
{
    private TestDataBuilder _builder;
    private readonly CampaignService _campaignService;
    private readonly CharacterService _characterService;

    public CharacterTests(DatabaseFixture fixture)
    {
        _builder = fixture.Builder;
        _campaignService = fixture.Builder.CampaignService;
        _characterService = fixture.Builder.CharacterService;
    }

    [Fact]
    public async Task Existing_campaign_should_be_initialized_with_default_characters()
    {
        // Arrange
        _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        // Act
        var campaign = await _campaignService.GetCampaignAsync(campaignId, CancellationToken.None);
        // Assert
        campaign.Characters.Should().NotBeNullOrEmpty();
        campaign.Characters.Should().HaveCount(3);
        campaign.Characters.Should().OnlyContain(c => c.Race is Menneske);
        campaign.Characters.Should().OnlyContain(c => c.Class is Bonde);
    }
    
    [Fact]
    public async Task Updating_character_background_succeed_with_valid_parameters()
    {
        // Arrange
        var testContext = _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        var campaign = testContext.Campaigns.First(c => c.Id.Equals(campaignId));
        var character = campaign.Characters.First();
        var oldBackgroundStory = character.BackgroundStory;
        await _characterService.GenerateCharacterBackstoryAsync(character, new[] {"test"});
        // Act
        await _characterService.UpdateCharacterAsync(character, CancellationToken.None);
        var newBackgroundStory = testContext.Campaigns.First().Characters.First().BackgroundStory;
        // Assert
        newBackgroundStory.Should().NotBeSameAs(oldBackgroundStory);
    }
    
    [Fact]
    public async Task Updating_existing_character_should_suceed_with_valid_parameters()
    {
        // Arrange
        _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        var campaign = await _campaignService.GetCampaignAsync(campaignId, CancellationToken.None);
        var character = campaign.Characters.First();
        var newRace = new Dværg();
        var newClass = new Barbar();
        character.Race = newRace;
        character.Class = newClass;
        
        // Act
        await _characterService.UpdateCharacterAsync(character, CancellationToken.None);
        
        // Assert
        character.Class.GetType().Should().Be(typeof(Barbar));
        character.Race.GetType().Should().Be(typeof(Dværg));
    }
}