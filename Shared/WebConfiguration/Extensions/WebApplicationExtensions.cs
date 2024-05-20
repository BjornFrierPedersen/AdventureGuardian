using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace WebConfiguration.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureBaseServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ConnectionFactory>();
    }
    
    public static T ConfigureOption<T>(this WebApplicationBuilder builder, string section) where T : class, new()
    {
        var optionsSection = builder.Configuration.GetSection(section);
        var options = new T();
        optionsSection.Bind(options);
        builder.Services.Configure<T>(optionsSection);
        return options;
    }

    public static void ApplyMigrations<T>(this WebApplication app) where T : DbContext
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<T>();
        context.Database.Migrate();
    }

    public static void AddRabbitMqConnection(this WebApplicationBuilder builder, string rabbitMqHost)
    {
        builder.Services
            .AddSingleton(_ => new ConnectionFactory
            {
                HostName = rabbitMqHost,
                DispatchConsumersAsync = true
            });
    }
}