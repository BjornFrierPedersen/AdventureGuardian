using System.ComponentModel.DataAnnotations.Schema;
using AdventureGuardian.Models.Models.Domain.Worlds;
using AdventureGuardian.Models.Models.Enums;
using Shared.Models;

namespace AdventureGuardian.Models.Models.Domain;

public class Campaign : BaseModel
{
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public virtual required World World { get; init; }
    public virtual required ICollection<Character> Characters { get; set; }
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    /// <summary>
    /// TODO: Make required when implemented
    /// </summary>
    [NotMapped]
    public Ruleset Ruleset { get; set; }

    public Encounter CreateEncounter(string name, List<int>? characterIds = null, Creatures[]? creatures = null)
    {
        var encounter = new Encounter
        {
            Name = name,
            CharacterIds = characterIds != null && characterIds.Any() ? 
                Characters.Select(character => character.Id).Where(characterIds.Contains).ToList() :
                Characters.Select(character => character.Id).ToList(),
            Creatures = creatures?.ToList() ?? World.Creatures.OrderBy(_ => Guid.NewGuid()).Take(2).ToList(),
            CampaignId = Id,
            Campaign = this
        };

        return encounter;
    }
}