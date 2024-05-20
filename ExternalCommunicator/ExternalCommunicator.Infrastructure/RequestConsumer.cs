using System.Text;
using MessageGateway;
using MessageGateway.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ExternalCommunicator.Infrastructure;

public class RequestConsumer : ConsumerBase, IHostedService
{
    protected override string OpenAiQueueAndExchangeRoutingKey => KnownProperties.RoutingKeyRequest;
    private readonly IServiceProvider _serviceProvider;
    
    public RequestConsumer(ConnectionFactory connectionFactory, IServiceProvider serviceProvider) : base(connectionFactory)
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
                var openAiCommunicatorService = scope.ServiceProvider.GetRequiredService<IOpenAiCommunicatorService>();
                var producer = scope.ServiceProvider.GetRequiredService<ResponseProducer>();
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var requestEvent = JsonConvert.DeserializeObject<Event>(body);
                var response = await openAiCommunicatorService.SendRequestAsync(requestEvent?.Message ?? throw new ArgumentNullException());
                producer.Publish(new ResponseEvent(requestEvent.EntityId, requestEvent.EntityType, response));
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving message from queue.\n{ex}");
        }
    }
}