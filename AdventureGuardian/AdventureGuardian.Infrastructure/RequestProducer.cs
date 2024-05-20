using MessageGateway;
using MessageGateway.Events;
using RabbitMQ.Client;

namespace AdventureGuardian.Infrastructure;

public class RequestProducer : ProducerBase<Event>
{
    public RequestProducer(ConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    protected override string RoutingKeyName => KnownProperties.RoutingKeyResponse;
    protected override string OpenAiQueueAndExchangeRoutingKey => KnownProperties.RoutingKeyResponse;
}