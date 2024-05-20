using ExternalCommunicator.Infrastructure;
using ExternalCommunicator.Infrastructure.Services;

namespace ExternalCommunicator;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Services.AddTransient<ResponseProducer>();
        // Tasks
        builder.Services.AddHostedService<RequestConsumer>();
        // DbContext
        builder.Services.AddDbContext<ExternalCommunicatorDbContext>();
        // Repositories
        // Services
        builder.Services.AddTransient<IOpenAiCommunicatorService, OpenAiCommunicatorService>();
    }
}