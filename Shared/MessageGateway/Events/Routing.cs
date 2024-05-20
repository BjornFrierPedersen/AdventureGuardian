namespace MessageGateway.Events;

public abstract class Routing
{
    public abstract string Key { get; }
    public abstract string QueueName { get; }
    
}

public class OpenAiRequestRoute : Routing
{
    private string baseName = "openai.request";
    public override string QueueName => "queue." + baseName;
    public override string Key => baseName;
}

public class OpenAiResponseRoute : Routing
{
    private string baseName = "openai.response";
    public override string QueueName => "queue." + baseName;
    public override string Key => baseName;
}