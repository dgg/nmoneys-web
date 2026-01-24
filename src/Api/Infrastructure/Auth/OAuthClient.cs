using Microsoft.Extensions.Options;

using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

internal class OAuthClient(HttpClient http) : IOAuthClient
{
	public async Task<OAuthToken> GetAccessToken(string clientId, string clientSecret, string audience, CancellationToken ct)
	{
		var body = new GetTokenBody(clientId, clientSecret, audience);
		using var response = await http.PostAsJsonAsync("token", body, ManagementSerializationContext.Default.Options, ct);
		OAuthToken token = await response.EnsureSuccessStatusCode().Content
				.ReadFromJsonAsync<OAuthToken>(ManagementSerializationContext.Default.Options, ct) ??
			throw new System.Text.Json.JsonException("Could not deserialize token from response.");

		return token;
	}
}