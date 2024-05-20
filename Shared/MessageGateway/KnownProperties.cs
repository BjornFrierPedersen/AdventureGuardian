namespace MessageGateway;

public class KnownProperties
{
    public static string VirtualHost => "adventureguardian";
    public static string AdventureGuardianExchange => $"{VirtualHost}.eventexchange";
    private static string OpenAiEvent => "openai.event";
    public static string OpenAiQueue => $"{VirtualHost}.{OpenAiEvent}";
    public static string RoutingKeyRequest => $"{OpenAiEvent}.request";
    public static string RoutingKeyResponse => $"{OpenAiEvent}.response";
    
    
}