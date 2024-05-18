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
    private readonly IMessagePublisher _publisher;
    
    public CharacterService(CharacterRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }
    
    public void GenerateCharacterBackstory(Character character, string[]? keywords = null)
    {
        _publisher.SendMessage(new RequestEvent(character.ExternalId, EntityType.Character, character.Prompt(keywords)),
            new OpenAiRequestRoute());
        character.BackgroundStory = KnownStringVariables.FetchingGeneratedResponse;
    }

    public async Task UpdateCharacterAsync(Character character, CancellationToken cancellationToken)
    {
        await _repository.UpdateCharacterAsync(character, cancellationToken);
    }
}