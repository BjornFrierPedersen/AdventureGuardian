using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Models.Models.Worlds;

namespace AdventureGuardian.Models.Models;

public class Campaign
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public virtual required World World { get; set; }
    public virtual required ICollection<Character> Characters { get; set; }
    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    /// <summary>
    /// TODO: Make required when implemented
    /// </summary>
    [NotMapped]
    public Ruleset Ruleset { get; set; }

    public Encounter CreateEncounter(string name, Creatures[]? creatures = null)
    {
        var encounter = new Encounter
        {
            Name = name,
            CharacterIds = Characters.Select(character => character.Id).ToList(),
            Creatures = creatures?.ToList() ?? World.Creatures.OrderBy(_ => Guid.NewGuid()).Take(2).ToList(),
            CampaignId = Id
        };

        return encounter;
    }
}