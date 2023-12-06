namespace AdventureGuardian.Models.Models.ClassModels;

public class Bonde : Class
{
    public override Enums.Classes ClassType => Enums.Classes.Bonde;

    public override Stats BaseStats { get; } = new()
    {
        Strength = 2,
        Charisma = 1,
        Constitution = 3,
        Dexterity = 2,
        Intelligence = 1,
        Wisdom = 2
    };

    public override int HitpointClassBonus => BaseStats.Constitution - 1;
}