namespace AdventureGuardian.Models.Models.Domain.ClassModels;

public class Barbar : Class
{
    public override Enums.Classes ClassType => Enums.Classes.Barbar;

    public override Stats BaseStats { get; } = new()
    {
        Strength = 5,
        Charisma = 3,
        Constitution = 4,
        Dexterity = 3,
        Intelligence = 2,
        Wisdom = 3
    };

    public override int HitpointClassBonus => BaseStats.Constitution;
}