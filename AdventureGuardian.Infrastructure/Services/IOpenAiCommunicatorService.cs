namespace AdventureGuardian.Infrastructure.Services;

public interface IOpenAiCommunicatorService
{
    Task<string> SendRequestAsync(string prompt);
}