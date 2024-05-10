using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Domain.Worlds;
using AdventureGuardian.Models.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class CampaignService
{
    private readonly WorldService _worldService;
    private readonly CampaignRepository _repository;

    public CampaignService(IOpenAiCommunicatorService openAiCommunicatorService, CampaignRepository repository)
    {
        _repository = repository;
        _worldService = new WorldService(openAiCommunicatorService);
    }

    public async Task<Campaign> CreateCampaignAsync(string campaignName, string worldName,
        (Gender sex, int age)[] playersByAge,
        World.WorldType worldType, bool displayExplicitContent, CancellationToken cancellationToken, string[]? keywords = null)
    {
        var world = await _worldService.GenerateWorldAsync(worldName, playersByAge.Select(_ => _.age).ToArray(),
            worldType, displayExplicitContent, keywords);
        var campaign = new Campaign
        {
            Name = campaignName,
            World = world,
            Characters = playersByAge.Select(player => new Character
            {
                Name = player.sex == Gender.Mand ? "Hr. Danmark" :
                    player.sex == Gender.Kvinde ? "Fru Danmark" : "Hen Danmark",
                Gender = player.sex
            }).ToList()
        };

        await _repository.CreateCampaignAsync(campaign, cancellationToken);

        return campaign;
    }

    public async Task<List<Campaign>> Campaigns(CancellationToken cancellationToken) => await _repository.Campaigns().ToListAsync(cancellationToken);

    public async Task<Campaign> GetCampaignAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.GetCampaignByIdAsync(id, cancellationToken);
    }

    public void DeleteCampaign(Campaign campaign)
    {
        //Campaigns.Remove(campaign);
    }

    public async Task UpdateCampaignAsync(Campaign campaign, CancellationToken cancellationToken)
    {
        await _repository.UpdateCampaignAsync(campaign, cancellationToken);
    }
}