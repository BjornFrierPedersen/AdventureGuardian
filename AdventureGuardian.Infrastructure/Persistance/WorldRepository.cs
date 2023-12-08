using AdventureGuardian.Models.Models.Worlds;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class WorldRepository
{
    private readonly AdventureGuardianDbContext _dbContext;

    public WorldRepository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteWorldAsync(World world, CancellationToken cancellationToken)
    {
        _dbContext.Worlds.Remove(world);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWorldAsync(World world, CancellationToken cancellationToken)
    {
        _dbContext.Worlds.Update(world);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<World> GetWorldByIdAsync(int id, CancellationToken cancellationToken)
    {
        var world = await _dbContext.Worlds.FirstOrDefaultAsync(world => world.Id.Equals(id), cancellationToken: cancellationToken);
        if (world is null) throw new ApplicationException("World not found");
        return world;
    }
}