using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class Repository
{
    private readonly AdventureGuardianDbContext _dbContext;
    public Repository(AdventureGuardianDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void MigrateDatabase()
    {
        _dbContext.Database.Migrate();
    }

    public async Task CleanDatabase(CancellationToken cancellationToken)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE \"Campaigns\",\"Worlds\",\"Encounters\",\"Stats\",\"Characters\" RESTART IDENTITY CASCADE", cancellationToken: cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}