using System.Text;
using MessageGateway.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace MessageGateway;

public interface IMessagePublisher
{
    void SendMessage(Event message, Routing routing);
}

public class RabbitMqPublisher : IMessagePublisher
{
    public void SendMessage(Event message, Routing routing)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "adventureguardian", 
            type: ExchangeType.Direct,
            durable: true);
        
        channel.QueueDeclare(queue: "openai-events",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "adventureguardian",
            routingKey: routing.Key,
            basicProperties: null,
            body: body);
    }
}