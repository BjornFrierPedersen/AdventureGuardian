namespace AdventureGuardian.Models.Models.Domain.ClassModels;

public abstract class Class
{
    public abstract Enums.Classes ClassType { get; }

    /// <summary>
    /// TODO: Make a switch based off of the ruleset
    /// </summary>
    public abstract Stats BaseStats { get; }

    public abstract int HitpointClassBonus { get; }
}