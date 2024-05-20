using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class CharacterRepository
{
    private readonly AdventureGuardianDbContext _dbContext;

    public CharacterRepository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateAsync(Character character, CancellationToken cancellationToken)
    {
        _dbContext.Characters.Update(character);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Character>?> GetCharactersByIdsAsync(List<int> characterIds, CancellationToken cancellationToken) =>
        await _dbContext.Characters.Where(c => characterIds.Contains(c.Id)).ToListAsync(cancellationToken);

    public async Task<Character> GetByIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        var character =
            await _dbContext.Characters.FirstOrDefaultAsync(character => character.ExternalId.Equals(externalId),
                cancellationToken);
        if (character is null) throw new ApplicationException("Character not found");
        return character;
    }
}