namespace AdventureGuardian.Infrastructure.Services.Domain;

public abstract class BaseAiGenerationService
{
    protected readonly IOpenAiCommunicatorService OpenAiCommunicatorService;

    protected BaseAiGenerationService(IOpenAiCommunicatorService openAiCommunicatorService)
    {
        OpenAiCommunicatorService = openAiCommunicatorService;
    }
}