using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Domain.RaceModels;

public class Menneske : Race
{
    public override Races RaceType => Races.Menneske;

    public override Stats BaseRacialBonus { get; } = new()
    {
        Strength = 1,
        Charisma = 1,
        Constitution = 0,
        Dexterity = 0,
        Intelligence = 1,
        Wisdom = 1
    };

    public override int BaseHitPoints => 10;
}