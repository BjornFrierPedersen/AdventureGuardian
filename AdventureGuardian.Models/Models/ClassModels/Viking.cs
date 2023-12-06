namespace AdventureGuardian.Models.Models.ClassModels;

public class Viking : Class
{
    public override Enums.Classes ClassType => Enums.Classes.Viking;

    public override Stats BaseStats { get; } = new()
    {
        Strength = 5,
        Charisma = 2,
        Constitution = 4,
        Dexterity = 3,
        Intelligence = 2,
        Wisdom = 3
    };

    public override int HitpointClassBonus => BaseStats.Constitution;
}