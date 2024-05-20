using RabbitMQ.Client;

namespace MessageGateway;

public abstract class RabbitMqClientBase : IDisposable
{
    protected abstract string OpenAiQueueAndExchangeRoutingKey { get; }
    protected IModel Channel { get; private set; }
    private IConnection _connection;
    private readonly ConnectionFactory _connectionFactory;

    protected RabbitMqClientBase(
        ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        ConnectToRabbitMq();
    }

    private void ConnectToRabbitMq()
    {
        if (_connection == null || _connection.IsOpen == false)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        if (Channel == null || Channel.IsOpen == false)
        {
            Channel = _connection.CreateModel();
            Channel.ExchangeDeclare(
                exchange: KnownProperties.AdventureGuardianExchange, 
                type: ExchangeType.Direct, 
                durable: true, 
                autoDelete: false);
                
            Channel.QueueDeclare(
                queue: KnownProperties.OpenAiQueue, 
                durable: false,
                exclusive: false, 
                autoDelete: false);
                
            Channel.QueueBind(
                queue: KnownProperties.OpenAiQueue, 
                exchange: KnownProperties.AdventureGuardianExchange, 
                routingKey: OpenAiQueueAndExchangeRoutingKey);
        }
    }

    public void Dispose()
    {
        try
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cannot dispose RabbitMQ channel or connection");
        }
    }
}