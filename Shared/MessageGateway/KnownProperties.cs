namespace MessageGateway;
public class KnownProperties
{
    public const string VirtualHost = "adventureguardian";
    public const string AdventureGuardianExchange = $"{VirtualHost}.eventexchange";
    private const string OpenAiEvent = "openai.events";
    public const string RoutingKeyRequest = $"{OpenAiEvent}.request";
    public const string RoutingKeyResponse = $"{OpenAiEvent}.response";
    public const string OpenAiRequestQueue = $"{VirtualHost}.{RoutingKeyRequest}.queue";
    public const string OpenAiResponseQueue = $"{VirtualHost}.{RoutingKeyResponse}.queue";
}