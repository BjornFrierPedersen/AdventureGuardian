using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Dto;
using AdventureGuardian.Models.Models.Domain;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class EncounterService : BaseAiGenerationService
{
    private readonly CampaignRepository _campaignRepository;
    private readonly CharacterRepository _characterRepository;
    public EncounterService(IOpenAiCommunicatorService openAiCommunicatorService, CampaignRepository campaignRepository, CharacterRepository characterRepository) : base(openAiCommunicatorService)
    {
        _campaignRepository = campaignRepository;
        _characterRepository = characterRepository;
    }
    
    public async Task<Encounter> CreateEncounterAsync(CreateEncounterDto createEncounterDto, CancellationToken cancellationToken)
    {
        var campaign = await _campaignRepository.GetCampaignByIdAsync(createEncounterDto.CampaignId, cancellationToken);
        var encounter = campaign.CreateEncounter(createEncounterDto.Name, createEncounterDto.Creatures);
        await GenerateEncounterDescriptionAsync(encounter, cancellationToken, createEncounterDto.CharacterIds);
        campaign.Encounters.Add(encounter);
        return encounter;
    }
    
    public async Task<Encounter> GenerateEncounterDescriptionAsync(Encounter encounter, CancellationToken cancellationToken, List<int>? characterIds = null)
    {
        var characters = characterIds != null ? await _characterRepository.GetCharactersByIdsAsync(characterIds, cancellationToken) : null;
        var encounterDescription = await OpenAiCommunicatorService.SendRequestAsync(encounter.Prompt(characters));
        encounter.Description = encounterDescription;
        return encounter;
    }
}