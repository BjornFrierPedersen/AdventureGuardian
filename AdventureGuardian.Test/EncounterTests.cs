using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Test.Database_Handling;
using FluentAssertions;
using Xunit;

namespace AdventureGuardian.Test;

[Collection(TestVariables.DatabaseCollection)]
public class EncounterTests
{
    private TestDataBuilder _builder;
    private readonly EncounterService _encounterService;

    public EncounterTests(DatabaseFixture fixture)
    {
        _builder = fixture.Builder;
        _encounterService = fixture.Builder.EncounterService;
    }
    
    [Fact]
    public async Task Generating_a_new_encounter_for_an_existing_campaign_should_succeed()
    {
        // Arrange
        var context = _builder
            .WithCampaign(out var campaignId)
            .WithEncounter(campaignId);
        var campaign = context.Campaigns.First(campaign => campaign.Id.Equals(campaignId));
        
        // Act
        await _encounterService.GenerateEncounterAsync("test_encounter", campaign);
        var newEncounter = context.Campaigns.First(c => c.Id.Equals(campaignId)).Encounters.Last();
        
        // Assert
        newEncounter.Created.Should().BeBefore(DateTime.UtcNow);
        newEncounter.CharacterIds.Should().HaveCount(3);
    }
}