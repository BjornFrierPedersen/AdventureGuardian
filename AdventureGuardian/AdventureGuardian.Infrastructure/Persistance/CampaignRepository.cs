using System.Security.Claims;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Domain.Worlds;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class CampaignRepository
{
    private readonly AdventureGuardianDbContext _dbContext;
    private readonly IClaimsHandlerService _claimsHandlerService;

    public CampaignRepository(AdventureGuardianDbContext dbContext, IClaimsHandlerService claimsHandlerService)
    {
        _dbContext = dbContext;
        _claimsHandlerService = claimsHandlerService;
    }

    public IQueryable<Campaign> Campaigns()
    {
        return _dbContext.Campaigns
            .Include(campaign => campaign.World)
            .Include(campaign => campaign.Encounters)
            .Include(campaign => campaign.Characters)
            .Where(campaign => campaign.UserId == _claimsHandlerService.GetClaim(ClaimTypes.NameIdentifier));
    }

    public async Task CreateCampaignAsync(Campaign campaign, CancellationToken cancellationToken)
    {
        await _dbContext.Campaigns.AddAsync(campaign, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCampaignAsync(Campaign campaign, CancellationToken cancellationToken)
    {
        _dbContext.Campaigns.Remove(campaign);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCampaignAsync(Campaign campaign, CancellationToken cancellationToken)
    {
        _dbContext.Campaigns.Update(campaign);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Campaign> GetCampaignByIdAsync(int id, CancellationToken cancellationToken)
    {
        var campaign = await Campaigns()
            .FirstOrDefaultAsync(campaign => campaign.Id.Equals(id), cancellationToken: cancellationToken);
        if (campaign is null) throw new ApplicationException("Campaign not found");
        return campaign;
    }
    
    public async Task<Campaign> GetCampaignByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        var campaign = await Campaigns()
            .FirstOrDefaultAsync(campaign => campaign.ExternalId.Equals(externalId), cancellationToken: cancellationToken);
        if (campaign is null) throw new ApplicationException("Campaign not found");
        return campaign;
    }

    public async Task<Encounter> GetEncounterByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        var encounter = await _dbContext.Encounters
            .FirstOrDefaultAsync(encounter => encounter.ExternalId.Equals(externalId), cancellationToken: cancellationToken);
        if (encounter is null) throw new ApplicationException("Encounter not found");
        return encounter;
    }

    public async Task UpdateEncounterAsync(Encounter encounter, CancellationToken cancellationToken)
    {
        _dbContext.Encounters.Update(encounter);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<World> GetWorldByIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        var world = await _dbContext.Worlds.FirstOrDefaultAsync(world => world.ExternalId.Equals(externalId), cancellationToken);
        if (world is null) throw new ApplicationException("World not found");
        return world;
    }
    
    public async Task UpdateWorldAsync(World world, CancellationToken cancellationToken)
    {
        _dbContext.Worlds.Update(world);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}