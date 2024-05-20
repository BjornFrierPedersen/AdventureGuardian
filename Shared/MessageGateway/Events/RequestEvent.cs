using Shared.Enums;

namespace MessageGateway.Events;

public class RequestEvent : Event
{
    public RequestEvent(Guid entityId, EntityType entityType, string message) : base(1, entityId, entityType, message)
    {
    }
}