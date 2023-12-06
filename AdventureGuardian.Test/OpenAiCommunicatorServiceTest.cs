using AdventureGuardian.Infrastructure.Services;
using FluentAssertions;
using Xunit;

namespace AdventureGuardian.Test;

public class OpenAiCommunicatorServiceTest
{
    [Fact]
    public void Assert_active_language_model_from_open_ai_is_initialized()
    {
        // Arrange
        var openAiCommunicatorService = new OpenAiCommunicatorService(false);
        var activeLanguageModelBefore = openAiCommunicatorService.ActiveLanguagemodel;
        // Act
        openAiCommunicatorService.GetFunctioningLanguageModel();
        var activeLanguageModelAfter = openAiCommunicatorService.ActiveLanguagemodel;
        // Assert
        activeLanguageModelBefore.Should().BeNullOrEmpty();
        activeLanguageModelAfter.Should().NotBeNullOrEmpty();
    }
}