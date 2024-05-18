using System.ComponentModel.DataAnnotations;
using Shared.Enums;
using Shared.Models;

namespace MessageGateway.Events;

public class Event : BaseModel
{
    public int VersionNumber { get; set; }
    public Guid EntityId { get; set; }
    public EntityType EntityType { get; set; }
    public string Message { get; set; }

    public Event(int versionNumber, Guid entityId, EntityType entityType, string message)
    {
        VersionNumber = versionNumber;
        EntityId = entityId;
        EntityType = entityType;
        Message = message;
    }
}