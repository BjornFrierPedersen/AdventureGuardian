using System.Security.Claims;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Dto;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class CampaignService
{
    private readonly WorldService _worldService;
    private readonly CampaignRepository _repository;
    private readonly IClaimsHandlerService _claimsHandlerService;

    public CampaignService(CampaignRepository repository, IClaimsHandlerService claimsHandlerService, WorldService worldService)
    {
        _repository = repository;
        _claimsHandlerService = claimsHandlerService;
        _worldService = worldService;
    }

    public async Task<Campaign> CreateCampaignAsync(CreateCampaignDto dto, CancellationToken cancellationToken)
    {
        var world = _worldService.GenerateWorld(dto.WorldName, dto.Players.Select(player => player.Age).ToArray(),
            dto.WorldType, dto.DisplayExplicitContent, dto.WorldKeywords);
        var campaign = new Campaign
        {
            Name = dto.CampaignName,
            UserId = _claimsHandlerService.GetClaim(ClaimTypes.NameIdentifier),
            World = world,
            Characters = dto.Players.Select(player => new Character
            {
                Name = player.Gender == Gender.Mand ? "Hr. Danmark" :
                    player.Gender == Gender.Kvinde ? "Fru Danmark" : "Hen Danmark",
                Gender = player.Gender
            }).ToList()
        };

        await _repository.CreateCampaignAsync(campaign, cancellationToken);

        return campaign;
    }

    public async Task<List<Campaign>> Campaigns(CancellationToken cancellationToken)
    {
        return await _repository.Campaigns().ToListAsync(cancellationToken);
    }

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