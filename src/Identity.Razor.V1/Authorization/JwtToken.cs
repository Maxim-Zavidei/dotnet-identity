using System.Text.Json.Serialization;

namespace Identity.Razor.V1.Authorization;

public class JwtToken
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    [JsonPropertyName("expires_at")]
    public required DateTime ExpiresAt { get; set; }
}
