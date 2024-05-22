using System.Text;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ExternalCommunicator.Infrastructure;

public class RequestConsumer : ConsumerBase
{
    protected override string QueueName => KnownProperties.OpenAiRequestQueue;
    protected override string QueueRoutingKey => KnownProperties.RoutingKeyRequest;
    private readonly IServiceProvider _serviceProvider;
    
    public RequestConsumer(ConnectionFactory connectionFactory, IServiceProvider serviceProvider) : base(connectionFactory)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            {
                var openAiCommunicatorService = scope.ServiceProvider.GetRequiredService<IOpenAiCommunicatorService>();
                var producer = scope.ServiceProvider.GetRequiredService<ResponseProducer>();
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var requestEvent = JsonConvert.DeserializeObject<Event>(body);
                if (string.IsNullOrEmpty(requestEvent?.Message.Trim()))
                    throw new ArgumentNullException(
                        "Message is null or empty. Cannot create prompt for the OpenAI API request.");
                var response =
                    await openAiCommunicatorService.SendRequestAsync(requestEvent?.Message ??
                                                                     throw new ArgumentNullException());
                producer.Publish(new ResponseEvent(requestEvent.EntityId, requestEvent.EntityType, response));
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