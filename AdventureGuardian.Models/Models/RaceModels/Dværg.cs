using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.RaceModels;

public class Dværg : Race
{
    public override Races RaceType => Races.Dværg;

    public override Stats BaseRacialBonus { get; } = new()
    {
        Strength = 2,
        Charisma = 0,
        Constitution = 2,
        Dexterity = 0,
        Intelligence = 0,
        Wisdom = 0
    };

    public override int BaseHitPoints => 16;
}