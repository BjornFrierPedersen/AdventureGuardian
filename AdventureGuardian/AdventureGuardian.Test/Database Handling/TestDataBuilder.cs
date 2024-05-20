using System.Security.Claims;
using AdventureGuardian.Infrastructure;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Infrastructure.Services.Domain;
using AdventureGuardian.Models.Dto;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Domain.Worlds;
using AdventureGuardian.Models.Models.Enums;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AdventureGuardian.Test.Database_Handling;

public class TestDataBuilder
{
    private readonly AdventureGuardianDbContext _dbContext;
    private readonly Repository _repository;
    public readonly CampaignService CampaignService;
    public readonly CharacterService CharacterService;
    public readonly EncounterService EncounterService;
    public readonly WorldService WorldService;

    // TODO: Reqrite this to use startup dependency injection: https://github.com/pengweiqhca/Xunit.DependencyInjection
    public TestDataBuilder(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
        _repository = new Repository(dbContext);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var publisherMock = new Mock<RequestProducer>();
        httpContextAccessorMock.Setup(m => m.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier))
            .Returns(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        publisherMock.Setup(m => m.Publish(It.IsAny<RequestEvent>()));
        var campaignRepository =
            new CampaignRepository(dbContext, new ClaimsHandlerService(httpContextAccessorMock.Object));
        WorldService = new WorldService(campaignRepository, publisherMock.Object);
        CharacterService = new CharacterService(new CharacterRepository(dbContext), publisherMock.Object);
        CampaignService = new CampaignService(campaignRepository, new ClaimsHandlerService(httpContextAccessorMock.Object), WorldService);
        EncounterService = new EncounterService(new CampaignRepository(dbContext, new ClaimsHandlerService(httpContextAccessorMock.Object)),
            new CharacterRepository(dbContext), publisherMock.Object);
    }

    public IQueryable<Campaign> Campaigns => _dbContext.Campaigns
        .Include(campaign => campaign.World)
        .Include(campaign => campaign.Encounters)
        .Include(campaign => campaign.Characters);


    public async Task CleanDatabaseAsync()
    {
        await _repository.CleanDatabase(CancellationToken.None);
    }

    public void SeedDefaultData()
    {
        WithCampaign(out var campaignId)
            .WithEncounter(campaignId)
            .Build();
    }

    public TestDataBuilder WithCampaign(out int campaignId, IEnumerable<Player>? players = null)
    {
        var createCampaignDto = new CreateCampaignDto("MyTestCampaignName", "Test world",
            players ?? new List<Player> { new(Gender.Kvinde, 4), new(Gender.Mand, 5), new(Gender.Mand, 7) },
            World.WorldType.Fantasy, false);
        campaignId = CampaignService.CreateAsync(createCampaignDto, CancellationToken.None).Result.Id;
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