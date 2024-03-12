using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AdventureGuardian.Models.Models.Enums;
using Newtonsoft.Json.Converters;

namespace AdventureGuardian.Models.Models.Worlds;

public abstract class World
{
    [Key] public int Id { get; set; }
    public abstract required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool ExplicitContent { get; private set; }

    [ForeignKey(nameof(Campaign))] public int CampaignId { get; set; }

    [JsonIgnore]
    public virtual Campaign Campaign { get; set; } = null!;

    public string Prompt(int[] playersByAge, string[]? keywords)
    {
        var sortedKeywords = keywords != null
            ? string.Join(", ", keywords)
            : string.Join(", ", Creatures.OrderBy(x => Guid.NewGuid()).Take(3));

        return
            $"Til brug i PnP - Beskriv en {Type} verden ved navn {Name}. " +
            $"Brug følgende nøgleord til at lave verdensopbygningen: {sortedKeywords}. " +
            $"Det skal være beskrivende og skal være tilrettet personer med følgende aldre: {string.Join(",", playersByAge)} " +
            $"Der skal være et overordnet mål som spillerne på sigt skal opnå";
    }

    public abstract WorldType Type { get; }
    [NotMapped] 
    [JsonPropertyName("AllowedRaces")] 
    public abstract List<Races> Races { get; }
    [NotMapped] 
    [JsonPropertyName("AllowedCreatures")] 
    public abstract List<Creatures> Creatures { get; }
    [NotMapped] 
    [JsonPropertyName("AllowedClasses")] 
    public abstract List<Classes> Classes { get; }

    public enum WorldType
    {
        Fantasy,
        SciFi,
        Realism
    }

    public World(bool shouldDisplayExplicitContent)
    {
        ExplicitContent = shouldDisplayExplicitContent;
    }

    public World()
    {
    }
}