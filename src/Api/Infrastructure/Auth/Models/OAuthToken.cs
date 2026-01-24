namespace NMoneys.Api.Infrastructure.Auth.Models;

public record OAuthToken(string AccessToken, uint ExpiresIn, string Scope, string TokenType)
{
	public TimeSpan Expiration => TimeSpan.FromSeconds(ExpiresIn);
}