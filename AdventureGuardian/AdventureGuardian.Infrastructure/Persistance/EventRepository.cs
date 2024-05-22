using AdventureGuardian.Models.Models.Domain.Worlds;
using MessageGateway.Events;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class EventRepository
{
    private readonly AdventureGuardianDbContext _dbContext;

    public EventRepository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateEvent(Event @event)
    {
        _dbContext.Events.Add(@event);
        _dbContext.SaveChanges();
    }
}