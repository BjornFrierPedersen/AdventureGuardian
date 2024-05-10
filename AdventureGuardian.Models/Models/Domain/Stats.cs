using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureGuardian.Models.Models.Domain;

public class Stats
{
    [Key] public int Id { get; set; }
    [ForeignKey(nameof(Character))] public int CharacterId { get; set; }
    public required int Strength { get; set; }
    public required int Dexterity { get; set; }
    public required int Constitution { get; set; }
    public required int Intelligence { get; set; }
    public required int Wisdom { get; set; }
    public required int Charisma { get; set; }
}