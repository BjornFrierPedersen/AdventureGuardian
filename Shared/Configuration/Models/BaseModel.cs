using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public abstract class BaseModel
{
    [Key] public int Id { get; set; }
    public Guid ExternalId { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; }
}