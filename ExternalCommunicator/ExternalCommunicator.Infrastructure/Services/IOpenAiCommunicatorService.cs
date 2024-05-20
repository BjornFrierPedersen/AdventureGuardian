namespace ExternalCommunicator;

public interface IOpenAiCommunicatorService
{
    Task<string> SendRequestAsync(string prompt);
}