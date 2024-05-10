using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class CharacterService : BaseAiGenerationService
{
    private readonly CharacterRepository _repository;
    public CharacterService(IOpenAiCommunicatorService openAiCommunicatorService, CharacterRepository repository) : base(openAiCommunicatorService)
    {
        _repository = repository;
    }
    
    public async Task GenerateCharacterBackstoryAsync(Character character, string[]? keywords = null)
    {
        var characterBackstroy = await OpenAiCommunicatorService.SendRequestAsync(character.Prompt(keywords));
        character.BackgroundStory = characterBackstroy;
    }

    public async Task UpdateCharacterAsync(Character character, CancellationToken cancellationToken)
    {
        await _repository.UpdateCharacterAsync(character, cancellationToken);
    }
}