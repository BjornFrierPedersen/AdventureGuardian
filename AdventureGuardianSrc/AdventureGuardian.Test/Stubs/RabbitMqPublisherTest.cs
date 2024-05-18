using MessageGateway;
using MessageGateway.Events;

namespace AdventureGuardian.Test.Stubs;

public class RabbitMqPublisherTest : IMessagePublisher
{
    public void SendMessage(Event message, Routing routing)
    {
        // Do nothing
    }
}