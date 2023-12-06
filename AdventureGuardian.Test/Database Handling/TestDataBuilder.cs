using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.ClassModels;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Models.Models.RaceModels;
using AdventureGuardian.Models.Models.Worlds;
using AdventureGuardian.Test.Stubs;
using Microsoft.EntityFrameworkCore;
using TinyHeroesRp.Services.Domain;

namespace AdventureGuardian.Test.Database_Handling;

public class TestDataBuilder
{
    private readonly AdventureGuardianDbContext _dbContext;
    private readonly Repository _repository = new();
    public readonly CampaignService CampaignService;

    public TestDataBuilder(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
        CampaignService =
            new CampaignService(new TestOpenAiCommunicatorService(), new CampaignRepository(dbContext));
    }

    public IQueryable<Campaign> Campaigns => _dbContext.Campaigns
        .Include(campaign => campaign.World)
        .Include(campaign => campaign.Encounters)
        .Include(campaign => campaign.Characters);


    public void CleanDatabase()
    {
        _repository.CleanDatabase();
    }

    public void SeedDefaultData()
    {
        WithCampaign(out var campaignId)
            .WithEncounter(campaignId)
            .Build();
    }


    public TestDataBuilder WithCampaign(out int campaignId, (Gender gender, int age)[]? playersByGenderAndAge = null)
    {
        campaignId = CampaignService
            .CreateCampaignAsync("MyTestCampaignName", "Test world",
                playersByGenderAndAge ?? new[] { (Gender.Kvinde, 4), (Gender.Mand, 5), (Gender.Mand, 7) },
                World.WorldType.Fantasy,
                false).Result.Id;
        return this;
    }

    public TestDataBuilder WithEncounter(int campaignId)
    {
        var campaign = Campaigns.FirstOrDefault(c => c.Id.Equals(campaignId)) ??
                       throw new ApplicationException("Campaign not found");
        var encounter = campaign.CreateEncounter("Test encounter");
        _dbContext.Encounters.Add(encounter);
        _dbContext.SaveChanges();
        return this;
    }

    public TestDataBuilder Build()
    {
        _dbContext.SaveChanges();
        return this;
    }
}