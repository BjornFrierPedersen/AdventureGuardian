using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Worlds;

public class FantasyWorld : World
{
    public override required string Name { get; set; }
    public override WorldType Type => WorldType.Fantasy;

    public override List<Races> Races { get; } = new()
    {
        Enums.Races.Menneske,
        Enums.Races.Dværg,
        Enums.Races.Elver,
        Enums.Races.Halvelver,
        Enums.Races.Hobbit,
        Enums.Races.Drage,
        Enums.Races.Kæmpe,
        Enums.Races.Goblin,
        Enums.Races.Ogre,
        Enums.Races.Ork,
        Enums.Races.Trold
    };

    public override List<Creatures> Creatures { get; } = new()
    {
        Enums.Creatures.Goblin,
        Enums.Creatures.Ork,
        Enums.Creatures.Trold,
        Enums.Creatures.Ogre,
        Enums.Creatures.Drage,
        Enums.Creatures.Kæmpe,
        Enums.Creatures.Slim,
        Enums.Creatures.Golem,
        Enums.Creatures.Skelet,
        Enums.Creatures.Zombie,
        Enums.Creatures.Vampyr,
        Enums.Creatures.Varulv,
        Enums.Creatures.Spøgelse,
        Enums.Creatures.Gru,
        Enums.Creatures.Ånd,
        Enums.Creatures.Banshee,
        Enums.Creatures.Dæmon,
        Enums.Creatures.Djævel
    };

    public override List<Classes> Classes { get; } = new()
    {
        Enums.Classes.Bonde,
        Enums.Classes.Barbar,
        Enums.Classes.Bard,
        Enums.Classes.Præst,
        Enums.Classes.Druid,
        Enums.Classes.Kriger,
        Enums.Classes.Munk,
        Enums.Classes.Paladin,
        Enums.Classes.Ranger,
        Enums.Classes.Tyv,
        Enums.Classes.Besværger,
        Enums.Classes.Sortekunstner,
        Enums.Classes.Troldmand,
        Enums.Classes.Psioniker
    };

    public FantasyWorld(bool shouldDisplayExplicitContent) : base(shouldDisplayExplicitContent)
    {
    }

    public FantasyWorld()
    {
    }
}