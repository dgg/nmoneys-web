using System.Net.Http.Headers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

internal class ManagementHttpHandler(IOAuthClient client, IMemoryCache cache, IOptions<AuthenticationSettings> configuration) : DelegatingHandler
{
	public static readonly string CacheKey = "ApplicationsManagerToken";
	
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		string accessToken = await getAccessToken(cancellationToken);
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		return await base.SendAsync(request, cancellationToken);
	}

	private async Task<string> getAccessToken(CancellationToken ct)
	{
		string? accessToken = await cache.GetOrCreateAsync(CacheKey, async entry =>
		{
			OAuthToken token = await client.GetAccessToken(configuration.Value.ApplicationsManager.ClientId,
				configuration.Value.ApplicationsManager.ClientSecret, 
				configuration.Value.Audience,
				ct);
			entry.AbsoluteExpirationRelativeToNow = token.Expiration;
			return token.AccessToken;
		});
		return accessToken!;
	}
}