namespace AdventureGuardian.Models.Models.Domain.ClassModels;

public class Troldmand : Class
{
    public override Enums.Classes ClassType => Enums.Classes.Troldmand;

    public override Stats BaseStats { get; } = new()
    {
        Strength = 2,
        Charisma = 3,
        Constitution = 2,
        Dexterity = 4,
        Intelligence = 5,
        Wisdom = 4
    };

    public override int HitpointClassBonus => BaseStats.Constitution;
}