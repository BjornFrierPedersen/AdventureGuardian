namespace TinyHeroesRp.Services;

public interface IOpenAiCommunicatorService
{
    Task<string> SendRequestAsync(string prompt);
}