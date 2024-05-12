using System.Security.Claims;
using AdventureGuardian.Infrastructure.Services;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
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
        var campaign = await _dbContext.Campaigns.FirstOrDefaultAsync(campaign => campaign.Id.Equals(id), cancellationToken: cancellationToken);
        if (campaign is null) throw new ApplicationException("Campaign not found");
        return campaign;
    }
}