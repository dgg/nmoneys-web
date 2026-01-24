namespace NMoneys.Api.Infrastructure.Auth.Models;

internal record CreateClientBody(string Name)
{
	public string AppType => "non_interactive";
	public bool IsFirstParty => false;
}