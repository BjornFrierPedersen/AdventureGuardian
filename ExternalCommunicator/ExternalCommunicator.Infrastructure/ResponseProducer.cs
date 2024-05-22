using ExternalCommunicator.Infrastructure.Persistance;
using MessageGateway;
using MessageGateway.Events;
using RabbitMQ.Client;

namespace ExternalCommunicator.Infrastructure;

public class ResponseProducer : ProducerBase<Event>
{
    protected override string QueueName { get; set; } = KnownProperties.OpenAiResponseQueue;
    private readonly EventRepository _eventRepository;
    public ResponseProducer(ConnectionFactory connectionFactory, EventRepository eventRepository) : base(connectionFactory)
    {
        _eventRepository = eventRepository;
    }

    protected override string RoutingKeyName => KnownProperties.RoutingKeyResponse;
    
    protected override void PersistEvent(Event @event)
    {
        _eventRepository.CreateEvent(@event);
    }
}