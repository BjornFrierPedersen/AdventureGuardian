using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureGuardian.Models.Models.ClassModels;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Models.Models.RaceModels;

namespace AdventureGuardian.Models.Models;

public class Character
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public required Gender Gender { get; set; }
    [NotMapped] public required Race Race { get; set; }
    public Races RaceType => Race.RaceType;
    [NotMapped] public required Class Class { get; set; }
    public Classes ClassType => Class.ClassType;
    public string BackgroundStory { get; set; } = string.Empty;
    public int Level { get; set; }
    public int BonusHitPoints { get; set; }
    [NotMapped] public int MaxHitPoints => BaseHitpoints + BonusHitPoints;
    public int HitPoints { get; set; }
    private int BaseHitpoints => Race.BaseHitPoints + Class.HitpointClassBonus;
    
    public string Prompt(string[]? keywords)
    {
        var keywordsPrompt = keywords != null
            ? $" Brug følgende nøgleord til at lave baggrundshistorien: {string.Join(", ", keywords)}"
            : string.Empty;

        return
            $"Lav en baggrundshistorie til en karakter ved navn {Name}. " +
            $"Karakteren er en {Gender} af racen {RaceType}, har klassen {ClassType} og er på level {Level}." +
            $"{keywordsPrompt}";
    }

    /// <summary>
    /// Used for tracking the bonus stats from items, spells, etc.
    /// </summary>
    public Stats BonusStats { get; set; } = new()
    {
        Strength = 0,
        Dexterity = 0,
        Constitution = 0,
        Intelligence = 0,
        Wisdom = 0,
        Charisma = 0
    };

    private Stats BaseStats
    {
        get
        {
            var baseStats = Class.BaseStats;
            var racialBonus = Race.BaseRacialBonus;
            return new Stats
            {
                Strength = baseStats.Strength + racialBonus.Strength,
                Dexterity = baseStats.Dexterity + racialBonus.Dexterity,
                Constitution = baseStats.Constitution + racialBonus.Constitution,
                Intelligence = baseStats.Intelligence + racialBonus.Intelligence,
                Wisdom = baseStats.Wisdom + racialBonus.Wisdom,
                Charisma = baseStats.Charisma + racialBonus.Charisma
            };
        }
    }

    [NotMapped]
    public Stats Stats
    {
        get
        {
            var baseStats = BaseStats;
            var bonusStats = BonusStats;
            return new Stats
            {
                Strength = baseStats.Strength + bonusStats.Strength,
                Dexterity = baseStats.Dexterity + bonusStats.Dexterity,
                Constitution = baseStats.Constitution + bonusStats.Constitution,
                Intelligence = baseStats.Intelligence + bonusStats.Intelligence,
                Wisdom = baseStats.Wisdom + bonusStats.Wisdom,
                Charisma = baseStats.Charisma + bonusStats.Charisma
            };
        }
    }
}