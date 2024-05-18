using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Domain.Worlds;

public class RealismWorld : World
{
    public override required string Name { get; set; }
    public override WorldType Type => WorldType.Realism;

    public override List<Races> Races { get; } = new()
    {
        Enums.Races.Nordling,
        Enums.Races.Sydling,
        Enums.Races.Vestling,
        Enums.Races.Østling
    };

    public override List<Creatures> Creatures { get; } = new()
    {
        Enums.Creatures.Elefant,
        Enums.Creatures.Løve,
        Enums.Creatures.Tiger,
        Enums.Creatures.Bjørn,
        Enums.Creatures.Uløve,
        Enums.Creatures.Ræv,
        Enums.Creatures.Hund,
        Enums.Creatures.Kat,
        Enums.Creatures.Hest,
        Enums.Creatures.Ko,
        Enums.Creatures.Gris,
        Enums.Creatures.Får,
        Enums.Creatures.Ged,
        Enums.Creatures.Flodhest,
        Enums.Creatures.Næsehorn,
        Enums.Creatures.Giraf,
        Enums.Creatures.Ulv
    };

    public override List<Classes> Classes { get; } = new()
    {
        Enums.Classes.Bonde,
        Enums.Classes.Snigmorder,
        Enums.Classes.Viking,
        Enums.Classes.Ingeniør,
        Enums.Classes.Infiltrator,
        Enums.Classes.Opdagelsesrejsende,
        Enums.Classes.Ranger,
        Enums.Classes.Munk,
        Enums.Classes.Smugler,
        Enums.Classes.Viking,
        Enums.Classes.Ridder,
        Enums.Classes.Præst,
        Enums.Classes.Alkymist
    };

    public RealismWorld(bool shouldDisplayExplicitContent) : base(shouldDisplayExplicitContent)
    {
    }

    public RealismWorld()
    {
    }
}