using System.Text;
using MessageGateway.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageGateway;

public interface IRabbitMqProducer<in T>
{
    void Publish(T @event);
}

public abstract class ProducerBase<T> : RabbitMqClientBase, IRabbitMqProducer<T>
{
    protected abstract string RoutingKeyName { get; }

    protected ProducerBase(
        ConnectionFactory connectionFactory) :
        base(connectionFactory)
    {
    }

    public virtual void Publish(T @event)
    {
        try
        {
            PersistEvent(@event);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            var properties = Channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1; // Doesn't persist to disk
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Channel.BasicPublish(exchange: KnownProperties.AdventureGuardianExchange, routingKey: RoutingKeyName, body: body, basicProperties: properties);
        }
        catch (Exception)
        {
            Console.WriteLine("Error while publishing");
        }
    }

    protected virtual void PersistEvent(T @event)
    {
        // Placeholder
    }
}