using MessageGateway;
using MessageGateway.Events;
using RabbitMQ.Client;

namespace ExternalCommunicator;

public class ResponseProducer : ProducerBase<Event>
{
    public ResponseProducer(ConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    protected override string RoutingKeyName => KnownProperties.RoutingKeyResponse;
    protected override string OpenAiQueueAndExchangeRoutingKey => KnownProperties.RoutingKeyRequest;
}