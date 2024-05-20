using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models;
using AdventureGuardian.Models.Dto;
using AdventureGuardian.Models.Models.Domain;
using MessageGateway;
using MessageGateway.Events;
using Shared.Enums;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class EncounterService
{
    private readonly CampaignRepository _campaignRepository;
    private readonly CharacterRepository _characterRepository;
    private readonly RequestProducer _requestProducer;
    
    public EncounterService(CampaignRepository campaignRepository, CharacterRepository characterRepository, RequestProducer requestProducer)
    {
        _campaignRepository = campaignRepository;
        _characterRepository = characterRepository;
        _requestProducer = requestProducer;
    }
    
    public async Task<Encounter> CreateEncounterAsync(CreateEncounterDto createEncounterDto, CancellationToken cancellationToken)
    {
        var campaign = await _campaignRepository.GetCampaignByIdAsync(createEncounterDto.CampaignId, cancellationToken);
        var encounter = campaign.CreateEncounter(createEncounterDto.Name, createEncounterDto.CharacterIds, createEncounterDto.Creatures);
        await GenerateEncounterDescriptionAsync(encounter, cancellationToken, createEncounterDto.CharacterIds);
        campaign.Encounters.Add(encounter);
        await _campaignRepository.UpdateCampaignAsync(campaign, cancellationToken);
        return encounter;
    }
    
    public async Task<Encounter> GenerateEncounterDescriptionAsync(Encounter encounter, CancellationToken cancellationToken, List<int>? characterIds = null)
    {
        var characters = characterIds != null ? await _characterRepository.GetCharactersByIdsAsync(characterIds, cancellationToken) : null;
        _requestProducer.Publish(new RequestEvent(encounter.ExternalId, EntityType.Encounter, encounter.Prompt(characters)));
        encounter.Description = KnownStringVariables.FetchingGeneratedResponse;
        return encounter;
    }
    
    public async Task<Encounter> GetByIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await _campaignRepository.GetEncounterByExternalIdAsync(externalId, cancellationToken);
    }
    
    public async Task UpdateAsync(Encounter encounter, CancellationToken cancellationToken)
    {
        await _campaignRepository.UpdateEncounterAsync(encounter, cancellationToken);
    }
}