namespace MessageGateway.Events;

public abstract class Routing
{
    public abstract string Key { get; }
    
}

public class OpenAiRequestRoute : Routing
{
    public override string Key => "openai.request";
}

public class OpenAiResponseRoute : Routing
{
    public override string Key => "openai.response";
}