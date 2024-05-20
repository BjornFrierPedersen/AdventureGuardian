using AdventureGuardian.Api.Startup;
using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Models.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebConfiguration.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Options
var idpOptions = builder.ConfigureOption<IdpOptions>(IdpOptions.Section);

// Configuration
builder.ConfigureBaseServices();
builder.ConfigureServices();
builder.AddRabbitMqConnection(builder.Configuration["RabbitMQHostName"]!);
builder.Services.AddOpenIdAuthentication(isLive: !builder.Environment.IsDevelopment(), idpOptions: idpOptions);
builder.AddCustomSwaggerGen();


var app = builder.Build();
app.MapCampaignEndpoints();
app.MapHelpEndpoints();
app.UseSwagger();
app.UseSwaggerUI(swaggerUiOptions =>
{
    swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "AdventureGuardian API");
    swaggerUiOptions.DocExpansion(DocExpansion.List);
    swaggerUiOptions.EnableDeepLinking();

    // Enable OAuth2 authorization support in Swagger UI
    swaggerUiOptions.OAuthClientId("adventureguardian");
    swaggerUiOptions.OAuthAppName("Swagger");
});
// Use routing middleware.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.ApplyMigrations<AdventureGuardianDbContext>();
app.Run();