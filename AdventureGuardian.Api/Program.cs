using AdventureGuardian.Api;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
builder.Services.AddSwaggerGen(c=>  
{  
    c.SwaggerDoc("v1", new OpenApiInfo  
    {  
        Version = "v1",
        Title = "AdventureGuardian API",
        Description = "An API for AdventureGuardian."
    });  
}); 
var app = builder.Build();
app.MapEndpoints();
app.UseSwagger();
app.UseSwaggerUI(c=>    
{    
    c.SwaggerEndpoint("/swagger/v1/swagger.json","AdventureGuardian API");    
});  
app.Run();