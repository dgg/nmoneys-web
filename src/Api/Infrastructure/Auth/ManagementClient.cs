using System.Net.Mail;

using Microsoft.Extensions.Options;

using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

internal class ManagementClient(HttpClient http, IOptions<AuthenticationSettings> settings) : IManagementClient
{
	public static Uri BaseUrl(string domain) => new UriBuilder(Uri.UriSchemeHttps, domain) { Path = "/api/v2/" }.Uri;


	public async Task<ApiClient> CreateApiClient(MailAddress email, CancellationToken ct)
	{
		var body = new CreateClientBody(email.Address);
		var response = await http.PostAsJsonAsync("clients", body, ManagementSerializationContext.Default.Options, ct);
		ApiClient client = await response.EnsureSuccessStatusCode().Content
				.ReadFromJsonAsync<ApiClient>(ManagementSerializationContext.Default.Options, ct) ??
			throw new System.Text.Json.JsonException("Could not deserialize client from response.");

		return client;
	}
	
	public async Task GrantApiClient(string clientId, CancellationToken ct)
	{
		var body = new GrantClientBody(clientId, settings.Value.Audience,
			// all scopes
			AuthorizationScope.List.Select(s => s.Value).ToArray());
		var response = await http.PostAsJsonAsync("client-grants", body, ManagementSerializationContext.Default.Options, ct);
		response.EnsureSuccessStatusCode();
	}
}