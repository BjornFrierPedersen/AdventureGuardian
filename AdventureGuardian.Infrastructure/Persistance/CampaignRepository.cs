using AdventureGuardian.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class CampaignRepository
{
    private readonly AdventureGuardianDbContext _dbContext;

    public CampaignRepository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Campaign> Campaigns()
    {
        return _dbContext.Campaigns
            .Include(campaign => campaign.World)
            .Include(campaign => campaign.Encounters)
            .Include(campaign => campaign.Characters);
    }

    public Campaign CreateCampaign(Campaign campaign)
    {
        _dbContext.Campaigns.Add(campaign);
        _dbContext.SaveChanges();
        return campaign;
    }

    public void DeleteCampaign(Campaign campaign)
    {
        _dbContext.Campaigns.Remove(campaign);
        _dbContext.SaveChanges();
    }

    public void UpdateCampaign(Campaign campaign)
    {
        _dbContext.Campaigns.Update(campaign);
        _dbContext.SaveChanges();
    }

    public Campaign GetCampaignById(int id)
    {
        var campaign = _dbContext.Campaigns.FirstOrDefault(campaign => campaign.Id.Equals(id));
        if (campaign is null) throw new ApplicationException("Campaign not found");
        return campaign;
    }
}