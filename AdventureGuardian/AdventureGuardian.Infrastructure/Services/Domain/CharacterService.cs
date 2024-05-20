using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models;
using AdventureGuardian.Models.Models.Domain;
using MessageGateway;
using MessageGateway.Events;
using Shared.Enums;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class CharacterService
{
    private readonly CharacterRepository _repository;
    private readonly RequestProducer _requestProducer;
    
    public CharacterService(CharacterRepository repository, RequestProducer requestProducer)
    {
        _repository = repository;
        _requestProducer = requestProducer;
    }
    
    public void GenerateCharacterBackstory(Character character, string[]? keywords = null)
    {
        _requestProducer.Publish(new RequestEvent(character.ExternalId, EntityType.Character, character.Prompt(keywords)));
        character.BackgroundStory = KnownStringVariables.FetchingGeneratedResponse;
    }

    public async Task UpdateAsync(Character character, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(character, cancellationToken);
    }
    
    public async Task<Character> GetByIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(externalId, cancellationToken);
    }
}