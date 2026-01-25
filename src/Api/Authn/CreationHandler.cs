using System.Net.Mail;

using FastEndpoints;

using NMoneys.Api.Infrastructure.Auth;

namespace NMoneys.Api.Authn;

internal record struct CreateClient(MailAddress Email) : ICommand<ClientCreationResponse>;

internal class CreationHandler(IManagementClient management) : ICommandHandler<CreateClient, ClientCreationResponse>
{
	public async Task<ClientCreationResponse> ExecuteAsync(CreateClient command, CancellationToken ct)
	{
		var client = await management.CreateApiClient(command.Email, ct);
		await management.GrantApiClient(client.ClientId, ct);
		
		var result = new ClientCreationResponse(client.ClientId, client.ClientSecret);
		return result;
	}
}