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

    public async Task UpdateCharacterAsync(Character character, CancellationToken cancellationToken)
    {
        _dbContext.Characters.Update(character);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Character>?> GetCharactersByIdsAsync(List<int> characterIds, CancellationToken cancellationToken) =>
        await _dbContext.Characters.Where(c => characterIds.Contains(c.Id)).ToListAsync(cancellationToken);

}