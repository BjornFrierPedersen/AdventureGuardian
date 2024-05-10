namespace AdventureGuardian.Models.Models.Options;

public class IdpOptions
{
    public const string Section = "Idp";
    public string Authority { get; set; } = "";
    public string Audience { get; set; } = "";
}