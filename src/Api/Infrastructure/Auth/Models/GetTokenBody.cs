namespace NMoneys.Api.Infrastructure.Auth.Models;

internal record GetTokenBody(string ClientId, string ClientSecret, string Audience)
{
	public string GrantType => "client_credentials";
}