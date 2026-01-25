using System.Net.Mail;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using NMoneys.Api.Infrastructure.OpenApi;
using NMoneys.Api.Infrastructure.Serialization;

namespace NMoneys.Api.Authn;

/// <summary>
/// Request to create a new API client.
/// </summary>
/// <param name="Email"></param>
internal sealed record ClientCreationRequest(MailAddress Email);

/// <summary>
/// Response containing the newly created client's credentials.
/// </summary>
/// <param name="ClientId">Unique identifier of the client.</param>
/// <param name="ClientSecret">String used to sign and validate tokens for the client.</param>
internal sealed record ClientCreationResponse(string ClientId, string ClientSecret)
	: IOpenApiSample<ClientCreationResponse>
{
	public static ClientCreationResponse Example { get; } = new("awZfdIir8YFdGZWkvCejDoUb7SjTDicx", "abc123XYZ789...");
}

internal sealed class
	ClientCreation : Endpoint<ClientCreationRequest, Results<Created<ClientCreationResponse>, ProblemDetails>>
{
	public override void Configure()
	{
		Post("/authn/clients");
		SerializerContext<SerializationContext>();
		AllowAnonymous();
		Summary(s =>
		{
			s.Summary = "Create API client";
			s.Description = "Creates a client that can communicate with the API";
			s.Response(201, "API client details", example: ClientCreationResponse.Example);
		});
	}

	public override async Task<Results<Created<ClientCreationResponse>, ProblemDetails>> ExecuteAsync(
		ClientCreationRequest req, CancellationToken ct)
	{
		var command = new CreateClient(req.Email);
		ClientCreationResponse response = await command.ExecuteAsync(ct);
		return TypedResults.Created(string.Empty, response);
	}
}