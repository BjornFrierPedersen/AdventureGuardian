using Shared.Enums;

namespace MessageGateway.Events;

public class ResponseEvent : Event
{
    public ResponseEvent(Guid entityId, EntityType entityType, string message) : base(1, entityId, entityType, message)
    {
    }
}