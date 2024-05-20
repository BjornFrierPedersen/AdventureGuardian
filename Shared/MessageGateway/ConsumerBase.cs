using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageGateway;

public abstract class ConsumerBase : RabbitMqClientBase
{
    public ConsumerBase(
        ConnectionFactory connectionFactory) : base(connectionFactory)
    {
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