using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace ExternalCommunicator.Infrastructure.Services;

public class OpenAiCommunicatorService : IOpenAiCommunicatorService
{
    public string ActiveLanguagemodel { get; private set; }

    private string[] LanguageModels => new[]
    {
        OpenAI.ObjectModels.Models.Gpt_4_turbo,
        OpenAI.ObjectModels.Models.Gpt_3_5_Turbo_Instruct,
        OpenAI.ObjectModels.Models.Gpt_3_5_Turbo_1106
    };

    private readonly OpenAIService _gptModel;

    public OpenAiCommunicatorService(IConfiguration configuration, bool initializeLanguageModel = true)
    {
        var apiKey = configuration["OPENAI_APIKEY"] ?? throw new ArgumentNullException("OpenAI API key not found");
        _gptModel = new(new OpenAiOptions
        {
            ApiKey = apiKey
        });
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
        var completionResult = await _gptModel.Completions.CreateCompletion(new CompletionCreateRequest()
        {
            Prompt = prompt,
            Model = languageModel,
            Temperature = 1,
            MaxTokens = 2048
        });

        return completionResult;
    }
}