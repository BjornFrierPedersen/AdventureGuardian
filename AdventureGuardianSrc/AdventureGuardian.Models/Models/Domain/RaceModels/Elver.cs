using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Domain.RaceModels;

public class Elver : Race
{
    public override Races RaceType => Races.Elver;

    public override Stats BaseRacialBonus { get; } = new()
    {
        Strength = 0,
        Charisma = 0,
        Constitution = 0,
        Dexterity = 2,
        Intelligence = 2,
        Wisdom = 1
    };

    public override int BaseHitPoints => 14;
}