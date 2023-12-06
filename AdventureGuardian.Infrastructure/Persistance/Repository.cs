using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class Repository
{
    public void MigrateDatabase()
    {
        using var dbContext = new AdventureGuardianDbContext();
        dbContext.Database.Migrate();
    }

    public void CleanDatabase()
    {
        using var dbContext = new AdventureGuardianDbContext();
        dbContext.Database.ExecuteSqlRaw(
            "TRUNCATE \"Campaigns\",\"Worlds\",\"Encounters\",\"Stats\",\"Characters\" RESTART IDENTITY CASCADE");
    }
}