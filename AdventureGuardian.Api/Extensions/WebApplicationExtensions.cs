using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Api.Extensions;

public static class WebApplicationExtensions
{
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
}