using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models;

public class Encounter
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public required List<Creatures> Creatures { get; init; }
    public required List<int> CharacterIds { get; init; }
    public string Description { get; set; } = string.Empty;
    [ForeignKey(nameof(Campaign))] public int CampaignId { get; set; }

    /// <summary>
    /// Navigational property for the world this encounter is part of.
    /// </summary>
    [JsonIgnore]
    public virtual Campaign Campaign { get; set; } = null!;

    public string Prompt(List<Character>? characters)
    {
        var sortedCharacterLevels = characters != null
            ? string.Join(", ", characters.Select(c => c.Level))
            : string.Join(", ", Campaign.Characters.Select(c => c.Level));
        var sortedCreatures = Creatures.Any()
            ? string.Join(", ", Creatures)
            : string.Join(", ", Campaign.World.Creatures.OrderBy(_ => Guid.NewGuid()).Take(2));

        return
            $"Til brug i PnP kampagne - Lav et encounter mod disse væsner: {sortedCreatures} for karakterer i levels: {sortedCharacterLevels}. " +
            $"Inkluder base stats for væsnerne, deres angreb og forsvar samt en indledning til encounter.";
    }
}