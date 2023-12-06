namespace AdventureGuardian.Models.Models.RaceModels;

public abstract class Race
{
    public abstract Enums.Races RaceType { get; }
    public abstract Stats BaseRacialBonus { get; }
    public abstract int BaseHitPoints { get; }
}