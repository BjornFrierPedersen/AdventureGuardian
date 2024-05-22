using MessageGateway.Events;

namespace ExternalCommunicator.Infrastructure.Persistance;

public class EventRepository
{
    private readonly ExternalCommunicatorDbContext _dbContext;

    public EventRepository(ExternalCommunicatorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateEvent(Event @event)
    {
        _dbContext.Events.Add(@event);
        _dbContext.SaveChanges();
    }
}