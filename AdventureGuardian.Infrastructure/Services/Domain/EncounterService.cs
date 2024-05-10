using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class EncounterService : BaseAiGenerationService
{
    public EncounterService(IOpenAiCommunicatorService openAiCommunicatorService) : base(openAiCommunicatorService)
    {
    }
    
    public async Task<Encounter> GenerateEncounterAsync(string name, Campaign campaign,
        List<Character>? characters = null, Creatures[]? creatures = null)
    {
        var encounter = campaign.CreateEncounter(name, creatures);
        await GenerateEncounterDescriptionAsync(encounter, characters);
        campaign.Encounters.Add(encounter);
        return encounter;
    }
    
    public async Task<Encounter> GenerateEncounterDescriptionAsync(Encounter encounter, List<Character>? characters = null)
    {
        var encounterDescription = await OpenAiCommunicatorService.SendRequestAsync(encounter.Prompt(characters));
        encounter.Description = encounterDescription;
        return encounter;
    }
}