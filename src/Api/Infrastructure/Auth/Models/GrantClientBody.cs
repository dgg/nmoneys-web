namespace NMoneys.Api.Infrastructure.Auth.Models;

internal record GrantClientBody(string ClientId, string Audience, string[] Scope)
{
	public string SubjectType => "client";
}