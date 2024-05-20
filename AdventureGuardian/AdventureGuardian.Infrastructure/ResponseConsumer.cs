using System.Text;
using AdventureGuardian.Infrastructure.Services.Domain;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Enums;

namespace AdventureGuardian.Infrastructure;

public class ResponseConsumer : ConsumerBase, IHostedService
{
    protected override string OpenAiQueueAndExchangeRoutingKey => KnownProperties.RoutingKeyResponse;
    private readonly IServiceProvider _serviceProvider;
    
    public ResponseConsumer(ConnectionFactory connectionFactory, IServiceProvider serviceProvider) : base(connectionFactory)
    {
        _serviceProvider = serviceProvider;
        try
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += OnEventReceived<Event>;
            Channel.BasicConsume(queue: KnownProperties.OpenAiQueue, autoAck: false, consumer: consumer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while consuming message: {ex}");
        }
    }
    
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    protected override async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(300000);
                var cancellationToken = cancellationTokenSource.Token;
                
                var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();
                var encounterService = scope.ServiceProvider.GetRequiredService<EncounterService>();
                var worldService = scope.ServiceProvider.GetRequiredService<WorldService>();
                
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var responseEvent = JsonConvert.DeserializeObject<ResponseEvent>(body);
                if (responseEvent == null) throw new ArgumentNullException();
                
                switch (responseEvent.EntityType)
                {
                    case EntityType.Character:
                        var character = await characterService.GetByIdAsync(responseEvent.ExternalId, cancellationToken);
                        character.BackgroundStory = responseEvent.Message;
                        await characterService.UpdateAsync(character, cancellationToken);
                        break;
                    case EntityType.Encounter:
                        var encounter = await encounterService.GetByIdAsync(responseEvent.EntityId, cancellationToken);
                        encounter.Description = responseEvent.Message;
                        await encounterService.UpdateAsync(encounter, cancellationToken);
                        break;
                    case EntityType.World:
                        var world = await worldService.GetByIdAsync(responseEvent.ExternalId, cancellationToken);
                        world.Description = responseEvent.Message;
                        await worldService.UpdateAsync(world, cancellationToken);
                        break;
                    default:
                        throw new ArgumentException($"Unknown entity type: {responseEvent.EntityType}");
                }
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving message from queue.\n{ex}");
        }
    }
}