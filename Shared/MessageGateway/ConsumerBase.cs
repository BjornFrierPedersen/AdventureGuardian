using MessageGateway.Events;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageGateway;

public abstract class ConsumerBase : RabbitMqClientBase, IHostedService
{
    protected override string QueueName { get; set; }
    protected abstract string QueueRoutingKey { get; }
    
    public ConsumerBase(ConnectionFactory connectionFactory) : base(connectionFactory)
    {
        Channel.QueueBind(
            queue: QueueName, 
            exchange: KnownProperties.AdventureGuardianExchange, 
            routingKey: QueueRoutingKey);
        try
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += OnEventReceived<Event>;
            Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
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
    
    protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            Console.WriteLine("Received message from queue");
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