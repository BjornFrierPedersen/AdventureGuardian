using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using TinyHeroesRp.Services;

namespace AdventureGuardian.Infrastructure.Services;

public class OpenAiCommunicatorService : IOpenAiCommunicatorService
{
    private const string OpenaiApiKey = "sk-br6m65W6GRE4G654REG564RE56G46ER51G61GR5G";
    public string ActiveLanguagemodel { get; private set; }

    private string[] LanguageModels => new[]
    {
        OpenAI.ObjectModels.Models.TextDavinciV3,
        OpenAI.ObjectModels.Models.Ada,
        OpenAI.ObjectModels.Models.Babbage,
        OpenAI.ObjectModels.Models.Curie
    };

    private readonly OpenAIService _gpt3 = new(new OpenAiOptions()
    {
        ApiKey = OpenaiApiKey
    });

    public OpenAiCommunicatorService(bool initializeLanguageModel = true)
    {
        if (initializeLanguageModel) GetFunctioningLanguageModel();
    }

    public async Task<string> SendRequestAsync(string prompt)
    {
        var completionResult = await CreateCompletionResult(prompt, ActiveLanguagemodel);

        if (completionResult.Successful)
            return string.Join("\n", completionResult.Choices.Select(choice => choice.Text).ToList());

        if (completionResult.Error == null) throw new Exception("Unknown Error");

        return $"{completionResult.Error.Code}: {completionResult.Error.Message}";
    }

    public void GetFunctioningLanguageModel()
    {
        foreach (var languageModel in LanguageModels)
        {
            var completionResult = CreateCompletionResult("reply yes", languageModel).Result;

            if (!completionResult.Successful) continue;
            ActiveLanguagemodel = languageModel;
            return;
        }

        if (string.IsNullOrEmpty(ActiveLanguagemodel))
            throw new ApplicationException("OpenAI API is not available for any of the chosen language models");
    }

    private async Task<CompletionCreateResponse> CreateCompletionResult(string prompt, string languageModel)
    {
        var completionResult = await _gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
        {
            Prompt = prompt,
            Model = languageModel,
            Temperature = 1,
            MaxTokens = 2048
        });

        return completionResult;
    }
}