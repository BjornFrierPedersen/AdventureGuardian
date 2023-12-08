using AdventureGuardian.Infrastructure.Persistance;
using Xunit;

namespace AdventureGuardian.Test.Database_Handling;

public class DatabaseFixture : IDisposable
{
    public TestDataBuilder Builder { get; set; }
    public AdventureGuardianDbContext AdventureGuardianDbContext { get; set; }

    public DatabaseFixture()
    {
        AdventureGuardianDbContext = new AdventureGuardianDbContext();
        Builder = new TestDataBuilder(AdventureGuardianDbContext);

        AdventureGuardianDbContext.Database.EnsureCreated();

        // We clear the data from the previously run test
        Builder.CleanDatabaseAsync();

        // And create the seed data to prepare for out own tests
        //Builder.SeedDefaultData();
    }

    public void Dispose()
    {
    }
}

[CollectionDefinition(TestVariables.DatabaseCollection)]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}