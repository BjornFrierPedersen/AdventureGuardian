using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Models.Worlds;

public class SciFiWorld : World
{
    public override required string Name { get; set; }
    public override WorldType Type => WorldType.SciFi;

    public override List<Races> Races { get; } = new()
    {
        Enums.Races.Bajoran,
        Enums.Races.Betazoid,
        Enums.Races.Cardassian,
        Enums.Races.Cyborg,
        Enums.Races.Ferengi,
        Enums.Races.Gungan,
        Enums.Races.Menneske,
        Enums.Races.Hutt,
        Enums.Races.Klingon,
        Enums.Races.Navi,
        Enums.Races.Saiyan,
        Enums.Races.Togruta,
        Enums.Races.Trill,
        Enums.Races.Twilek,
        Enums.Races.Vulcan,
        Enums.Races.Wookiee,
        Enums.Races.Zabrak
    };

    public override List<Creatures> Creatures { get; } = new()
    {
        Enums.Creatures.Xenomorphs,
        Enums.Creatures.Syntetikere,
        Enums.Creatures.Tomrumsvandrere,
        Enums.Creatures.Nanitessværme,
        Enums.Creatures.Muterede_væsener,
        Enums.Creatures.Augmenterede_væsener,
        Enums.Creatures.Energifantomer,
        Enums.Creatures.Kybernetiske_hybrider
    };

    public override List<Classes> Classes { get; } = new()
    {
        Enums.Classes.Bonde,
        Enums.Classes.Alienist,
        Enums.Classes.Artificer,
        Enums.Classes.Snigmorder,
        Enums.Classes.Astrobiolog,
        Enums.Classes.Tidsmagiker,
        Enums.Classes.Cybernetiker,
        Enums.Classes.Ingeniør,
        Enums.Classes.Opdagelsesrejsende,
        Enums.Classes.Infiltrator,
        Enums.Classes.Navigator,
        Enums.Classes.Medic,
        Enums.Classes.Spejder,
        Enums.Classes.Smugler,
        Enums.Classes.Teknomancer,
        Enums.Classes.Telepat,
        Enums.Classes.Dusørjæger,
        Enums.Classes.Robotikspecialist,
        Enums.Classes.Rummarine,
        Enums.Classes.Stjerneskibspilot
    };

    public SciFiWorld(bool shouldDisplayExplicitContent) : base(shouldDisplayExplicitContent)
    {
    }

    public SciFiWorld()
    {
    }
}