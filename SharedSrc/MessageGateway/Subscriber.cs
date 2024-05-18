using System.Text;
using MessageGateway.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageGateway;

public interface IMessageSubscriber
{
    void ConsumeMessage(Action<Event> action);
}

public class RabbitMqSubscriber<TRoute> : IMessageSubscriber where TRoute : Routing
{
    private readonly Routing _routing;
    public RabbitMqSubscriber()
    {
        _routing = Activator.CreateInstance<TRoute>();
    }
    
   public void ConsumeMessage(Action<Event> action)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _routing.Key,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var eventMessage = JsonConvert.DeserializeObject<Event>(json);
            if (eventMessage == null) throw new ArgumentNullException();
            action.Invoke(eventMessage);
        };
        channel.BasicConsume(queue: _routing.Key,
            autoAck: true,
            consumer: consumer);
    }
}