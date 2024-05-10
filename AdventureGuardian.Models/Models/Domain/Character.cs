using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureGuardian.Models.Extensions;
using AdventureGuardian.Models.Models.Domain.ClassModels;
using AdventureGuardian.Models.Models.Domain.RaceModels;
using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Domain;

public class Character
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public required Gender Gender { get; set; }

    [NotMapped]
    public Race Race
    {
        // TODO: Get this as a singleton from a Races access class
        get
        {
            var races = ReflectiveEnumerator.GetEnumerableOfType<Race>();
            return races.First(race => race.RaceType.Equals(RaceType));
        }
    }

    public Races RaceType { get; set; } = Races.Menneske;

    [NotMapped]
    public Class Class
    {
        // TODO: Get this as a singleton from a Classes access class
        get
        {
            var classes = ReflectiveEnumerator.GetEnumerableOfType<Class>();
            return classes.First(customClass => customClass.ClassType.Equals(ClassType));
        }
    }

    public Classes ClassType { get; set; } = Classes.Bonde;
    public string BackgroundStory { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
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