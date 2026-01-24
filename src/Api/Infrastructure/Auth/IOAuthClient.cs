using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

public interface IOAuthClient
{
	Task<OAuthToken> GetAccessToken(string clientId, string clientSecret, string audience, CancellationToken ct);
}