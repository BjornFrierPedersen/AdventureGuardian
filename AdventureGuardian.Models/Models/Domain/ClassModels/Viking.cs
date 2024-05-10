namespace AdventureGuardian.Models.Models.Domain.ClassModels;

public class Viking : Class
{
    public override Enums.Classes ClassType => Enums.Classes.Viking;

    public override Stats BaseStats { get; } = new()
    {
        Strength = 4,
        Charisma = 2,
        Constitution = 5,
        Dexterity = 4,
        Intelligence = 2,
        Wisdom = 3
    };

    public override int HitpointClassBonus => BaseStats.Constitution;
}