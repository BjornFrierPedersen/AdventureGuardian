using System.Net;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AdventureGuardian.Api.Helpers;

public class TokenService
{
    public async Task<string> GetDevelopmentToken(string username, string password, CancellationToken cancellationToken)
    {
        // Create an instance of HttpClient
        using var client = new HttpClient();

        // Set the base URL
        var baseUrl = "http://localhost:28080/realms/adventureguardian/protocol/openid-connect/token";

        // Prepare the form data
        var formData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "adventureguardian"),
            new KeyValuePair<string, string>("client_secret", "PZlbVN5O3Lr0FEWE1Mo8fmcwdw1z5DAu"),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        });

        // Send the POST request
        var response = await client.PostAsync(baseUrl, formData, cancellationToken);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken: cancellationToken);
            return token?.access_token ?? throw new ApplicationException("Access token was empty");
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        throw new ApplicationException("An error occurred while trying to get the token");
    }
}

public class AccessTokenResponse
{
     public string access_token { get; set; }
     public int expires_in { get; set; }
     public int refresh_expires_in { get; set; }
     public string refresh_token { get; set; }
     public string token_type { get; set; }
     public int not_before_policy { get; set; }
     public string session_state { get; set; }
     public string scope { get; set; }
     public ResourceAccess resource_access { get; set; }
     public string name { get; set; }
     public string given_name { get; set; }
     public string family_name { get; set; }
     public string email { get; set; }
}

public class ResourceAccess
{
    public List<string> realm_access { get; set; }
}