using AdventureGuardian.Models.Models.Worlds;

namespace AdventureGuardian.Infrastructure.Persistance;

public class WorldRepository
{
    public World CreateWorld(World world)
    {
        using var dbContext = new AdventureGuardianDbContext();
        dbContext.Worlds.Add(world);
        dbContext.SaveChanges();
        return world;
    }

    public void DeleteWorld(World world)
    {
        using var dbContext = new AdventureGuardianDbContext();
        dbContext.Worlds.Remove(world);
        dbContext.SaveChanges();
    }

    public void UpdateWorld(World world)
    {
        using var dbContext = new AdventureGuardianDbContext();
        dbContext.Worlds.Update(world);
        dbContext.SaveChanges();
    }

    public World GetWorldById(int id)
    {
        using var dbContext = new AdventureGuardianDbContext();
        var world = dbContext.Worlds.FirstOrDefault(world => world.Id.Equals(id));
        if (world is null) throw new ApplicationException("World not found");
        return world;
    }
}