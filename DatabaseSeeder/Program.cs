

using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Test.Database_Handling;
using Microsoft.EntityFrameworkCore;

await using var dbContext = new AdventureGuardianDbContext();
var dataBuilder = new TestDataBuilder(dbContext);
await dbContext.Database.MigrateAsync();
await dataBuilder.CleanDatabaseAsync();
dataBuilder.SeedDefaultData();
Console.WriteLine("Database seeded with default data!");