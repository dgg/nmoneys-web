using FastEndpoints;

using Microsoft.Extensions.Options;

using NMoneys.Api.Infrastructure.Auth;
using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Authn;

internal record struct RetrieveToken(string ClientId, string ClientSecret) : ICommand<TokenRetrievalResponse>;

internal class RetrievalHandler(IOAuthClient oauth, IOptions<AuthenticationSettings> settings) : ICommandHandler<RetrieveToken, TokenRetrievalResponse>
{
	public async Task<TokenRetrievalResponse> ExecuteAsync(RetrieveToken command, CancellationToken ct)
	{
		OAuthToken token = await oauth.GetAccessToken(command.ClientId, command.ClientSecret, settings.Value.Audience, ct);
		string[] scopes = token.Scope.Split(' ');
		var result = new TokenRetrievalResponse(token.AccessToken, token.ExpiresIn, scopes, token.TokenType);
		return result;
	}
}