using System.Text;
using AdventureGuardian.Infrastructure.Services.Domain;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Enums;

namespace AdventureGuardian.Infrastructure;

public class ResponseConsumer : ConsumerBase
{
    protected override string QueueName => KnownProperties.OpenAiResponseQueue;
    protected override string QueueRoutingKey => KnownProperties.RoutingKeyResponse;
    private readonly IServiceProvider _serviceProvider;
    
    public ResponseConsumer(ConnectionFactory connectionFactory, IServiceProvider serviceProvider) : base(connectionFactory)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(5000);
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
                        var character = await characterService.GetByIdAsync(responseEvent.EntityId, cancellationToken);
                        character.BackgroundStory = responseEvent.Message;
                        await characterService.UpdateAsync(character, cancellationToken);
                        break;
                    case EntityType.Encounter:
                        var encounter = await encounterService.GetByIdAsync(responseEvent.EntityId, cancellationToken);
                        encounter.Description = responseEvent.Message;
                        await encounterService.UpdateAsync(encounter, cancellationToken);
                        break;
                    case EntityType.World:
                        var world = await worldService.GetByIdAsync(responseEvent.EntityId, cancellationToken);
                        world.Description = responseEvent.Message;
                        await worldService.UpdateAsync(world, cancellationToken);
                        break;
                    default:
                        throw new ArgumentException($"Unknown entity type: {responseEvent.EntityType}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving message from queue.\n{ex}");
        }
        finally
        {
            Channel.BasicAck(@event.DeliveryTag, false);
        }
    }
}