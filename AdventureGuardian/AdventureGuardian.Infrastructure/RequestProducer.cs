using AdventureGuardian.Infrastructure.Persistance;
using MessageGateway;
using MessageGateway.Events;
using RabbitMQ.Client;

namespace AdventureGuardian.Infrastructure;

public class RequestProducer : ProducerBase<Event>
{
    protected override string QueueName { get; set; } = KnownProperties.OpenAiRequestQueue;
    private readonly EventRepository _eventRepository;
    public RequestProducer(ConnectionFactory connectionFactory, EventRepository eventRepository) : base(connectionFactory)
    {
        _eventRepository = eventRepository;
    }
    
    protected override string RoutingKeyName => KnownProperties.RoutingKeyRequest;

    protected override void PersistEvent(Event @event)
    {
        _eventRepository.CreateEvent(@event);
    }
}