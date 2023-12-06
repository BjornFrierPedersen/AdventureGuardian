using TinyHeroesRp.Services;

namespace AdventureGuardian.Test.Stubs;

public class TestOpenAiCommunicatorService : IOpenAiCommunicatorService
{
    public Task<string> SendRequestAsync(string prompt)
    {
        return Task.FromResult("This is a test");
    }
}