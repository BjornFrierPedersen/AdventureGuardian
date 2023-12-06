using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.RaceModels;

public class Human : Race
{
    public override Races RaceType => Races.Menneske;

    public override Stats BaseRacialBonus { get; } = new()
    {
        Strength = 0,
        Charisma = 1,
        Constitution = 0,
        Dexterity = 0,
        Intelligence = 1,
        Wisdom = 0
    };

    public override int BaseHitPoints => 12;
}