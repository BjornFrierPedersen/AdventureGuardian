using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Models.Domain.ClassModels;
using AdventureGuardian.Models.Models.Domain.RaceModels;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Test.Database_Handling;
using FluentAssertions;
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
        // Act
        await _characterService.GenerateCharacterBackstoryAsync(character, new[] {"test"});
        await _characterService.UpdateCharacterAsync(character, CancellationToken.None);
        // Assert
        var campaignAfterUpdate = await _campaignService.GetCampaignAsync(campaignId, CancellationToken.None);
        var newBackgroundStory = campaignAfterUpdate.Characters.First().BackgroundStory;
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
        character.RaceType = Races.Dværg;
        character.ClassType = Classes.Barbar;
        
        // Act
        await _characterService.UpdateCharacterAsync(character, CancellationToken.None);
        
        // Assert
        character.Class.GetType().Should().Be(typeof(Barbar));
        character.Race.GetType().Should().Be(typeof(Dværg));
    }
}