using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Models.Models.Worlds;
using AdventureGuardian.Test.Database_Handling;
using AdventureGuardian.Test.Stubs;
using FluentAssertions;
using TinyHeroesRp.Services.Domain;
using Xunit;

namespace AdventureGuardian.Test;

[Collection(TestVariables.DatabaseCollection)]
public class CampaignTests
{
    private AdventureGuardianDbContext _dbContext;
    private TestDataBuilder _builder;
    private readonly CampaignService _controller;

    public CampaignTests(DatabaseFixture fixture)
    {
        _builder = fixture.Builder;
        _dbContext = fixture.AdventureGuardianDbContext;
        _controller = fixture.Builder.CampaignService;
    }

    [Fact]
    public async Task When_creating_a_new_campaign_with_valid_parameters_it_should_be_created()
    {
        // Nu er vi ved kampagne overblikssiden
        // Her indtaster vi kampagnenavn, verdenstype- og navn, antal spillere og deres køn og alder.
        var campaignName = "MyTestCampaignName";
        var worldType = World.WorldType.Fantasy;
        var worldName = "Test world";
        var playersBySexAndAge = new[] { (Gender.Kvinde, 4), (Gender.Mand, 5), (Gender.Mand, 7) };
        // Her trykker vi på gem
        var campaign =
            await _controller.CreateCampaignAsync(campaignName, worldName, playersBySexAndAge, worldType, false);
        var newlyCreatedCampaign = _controller.GetCampaign(campaign.Id);
        // Her er vi tilbage på kampagne overblikssiden hvor vi kan se vores nyoprettede kampagne
        newlyCreatedCampaign.Should().NotBeNull();
    }

    [Fact]
    public void When_creating_a_new_campaign_with_invalid_parameters_it_should_throw_exception()
    {
        // Nu er vi ved kampagne overblikssiden
        // Her indtaster vi kampagnenavn, verdenstype- og navn, antal spillere og deres køn og alder.
        var campaignName = "MyTestCampaignName";
        var worldType = World.WorldType.Fantasy;
        var worldName = "Test world";
        var playersBySexAndAge = new[] { (Gender.Kvinde, 3), (Gender.Mand, 5), (Gender.Mand, 7) };
        // Her trykker vi på gem
        var invalidCampaignCreationAction = () => _controller.CreateCampaignAsync(campaignName, worldName,
            playersBySexAndAge, worldType,
            false).Result;
        // Ger er vi stadig på opret kampagne siden hvor den viser nedenstående fejlbesked
        invalidCampaignCreationAction.Should().Throw<ArgumentException>()
            .WithMessage("Players must be at least 4 years old to play this game");
    }

    [Fact]
    public void When_editing_an_existing_campaign_with_valid_parameters_it_should_update_the_campaign()
    {
        // Nu er vi på kampagne overblikssiden
        _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        var campaign = _controller.GetCampaign(campaignId);
        // Nu klikker vi på rediger på den kampagne vi lige har oprettet og ændrer navnet
        var changedCampaignName = "My changed campaign name";
        campaign.Name = changedCampaignName;
        // Nu trykker vi på gem
        _controller.UpdateCampaign(campaign);
        var updatedCampaignName = _controller.GetCampaign(campaignId).Name;
        // Nu er vi tilbage på kampagne overblikssiden hvor vi kan se vores ændrede kampagne
        updatedCampaignName.Should().BeSameAs(changedCampaignName);
    }
}