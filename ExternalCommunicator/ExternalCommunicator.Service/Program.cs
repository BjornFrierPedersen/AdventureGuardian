using ExternalCommunicator;
using ExternalCommunicator.Infrastructure;
using WebConfiguration.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureBaseServices();
builder.ConfigureServices();
builder.AddRabbitMqConnection(builder.Configuration["RabbitMQHostName"]!);
var app = builder.Build();
app.ApplyMigrations<ExternalCommunicatorDbContext>();
app.Run();