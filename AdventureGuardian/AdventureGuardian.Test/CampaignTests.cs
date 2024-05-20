using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Dto;
using AdventureGuardian.Models.Models.Domain.Worlds;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Test.Database_Handling;
using FluentAssertions;
using Xunit;

namespace AdventureGuardian.Test;

[Collection(TestVariables.DatabaseCollection)]
public class CampaignTests
{
    private TestDataBuilder _builder;
    private readonly CampaignService _campaignService;

    public CampaignTests(DatabaseFixture fixture)
    {
        _builder = fixture.Builder;
        _campaignService = fixture.Builder.CampaignService;
    }

    [Fact]
    public async Task When_creating_a_new_campaign_with_valid_parameters_it_should_be_created()
    {
        // Arrange
        var campaignName = "MyTestCampaignName";
        var worldType = World.WorldType.Fantasy;
        var worldName = "Test world";
        var players = new List<Player> { new(Gender.Kvinde, 4), new(Gender.Mand, 5), new(Gender.Mand, 7) };
        // Act
        var campaign =
            await _campaignService.CreateAsync(new CreateCampaignDto(campaignName, worldName, players, worldType, false), CancellationToken.None);
        var newlyCreatedCampaign = await _campaignService.GetByIdAsync(campaign.Id, CancellationToken.None);
        // Assert
        newlyCreatedCampaign.Should().NotBeNull();
    }

    [Fact]
    public void When_creating_a_new_campaign_with_invalid_parameters_it_should_throw_exception()
    {
        // Arrange
        var campaignName = "MyTestCampaignName";
        var worldType = World.WorldType.Fantasy;
        var worldName = "Test world";
        var players = new List<Player> { new(Gender.Kvinde, 3), new(Gender.Mand, 5), new(Gender.Mand, 7) };
        // Act
        var invalidCampaignCreationAction = () => _campaignService.CreateAsync(new CreateCampaignDto(campaignName, worldName,
            players, worldType, false), CancellationToken.None).Result;
        // Assert
        invalidCampaignCreationAction.Should().Throw<ArgumentException>()
            .WithMessage("Players must be at least 4 years old to play this game");
    }

    [Fact]
    public async Task When_editing_an_existing_campaign_with_valid_parameters_it_should_update_the_campaign()
    {
        // Arrange
        _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        var campaign = await _campaignService.GetByIdAsync(campaignId, CancellationToken.None);
        var changedCampaignName = "My changed campaign name";
        campaign.Name = changedCampaignName;
        // Act
        await _campaignService.UpdateAsync(campaign, CancellationToken.None);
        var updatedCampaign = await _campaignService.GetByIdAsync(campaignId, CancellationToken.None);
        // Assert
        updatedCampaign.Name.Should().BeSameAs(changedCampaignName);
    }
}