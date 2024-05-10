using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;

namespace AdventureGuardian.Infrastructure.Persistance;

public class CharacterRepository
{
    private readonly AdventureGuardianDbContext _dbContext;

    public CharacterRepository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task UpdateCharacterAsync(Character character, CancellationToken cancellationToken)
    {
        _dbContext.Characters.Update(character);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}