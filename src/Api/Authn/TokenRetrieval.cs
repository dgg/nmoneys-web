using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using NMoneys.Api.Infrastructure.Auth;
using NMoneys.Api.Infrastructure.OpenApi;
using NMoneys.Api.Infrastructure.Serialization;

namespace NMoneys.Api.Authn;

/// <summary>
/// Request to retrieve an access token.
/// </summary>
/// <param name="ClientId">Unique identifier of the client.</param>
/// <param name="ClientSecret">String used to sign and validate tokens for the client.</param>
internal sealed record TokenRetrievalRequest(string ClientId, string ClientSecret)
	: IOpenApiSample<TokenRetrievalRequest>
{
	public static TokenRetrievalRequest Example { get; } = new("awZfdIir8YFdGZWkvCejDoUb7SjTDicx", "abc123XYZ789...");
}

/// <summary>
/// Response containing the newly created client's credentials.
/// </summary>
/// <param name="AccessToken">Token conforming to the JWT standard that grants access to the secured parts of the API.</param>
/// <param name="ExpiresInSeconds">Validity of the access token (in seconds).</param>
/// <param name="Scopes">The scopes that a </param>
/// <param name="Type"></param>
internal sealed record TokenRetrievalResponse(string AccessToken, uint ExpiresInSeconds, string[] Scopes, string Type)
	: IOpenApiSample<TokenRetrievalResponse>
{
	public static TokenRetrievalResponse Example { get; } = new(
		"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFaenFndDJkLVRP...", 86400, [AuthorizationScope.ReadCurrencies],
		"Bearer");
}

internal sealed class
	TokenRetrieval : Endpoint<TokenRetrievalRequest, Results<Ok<TokenRetrievalResponse>, ProblemDetails>>
{
	public override void Configure()
	{
		Post("/authn/tokens");
		SerializerContext<SerializationContext>();
		AllowAnonymous();
		Summary(s =>
		{
			s.Summary = "Gets an access token that grants access to the API";
			s.Description = "Retrieves information about the token that can be used to call the API.";
			s.Response(200, "Access token for the API", example: TokenRetrievalResponse.Example);
		});
	}

	public override async Task<Results<Ok<TokenRetrievalResponse>, ProblemDetails>> ExecuteAsync(
		TokenRetrievalRequest req, CancellationToken ct)
	{
		var command = new RetrieveToken(req.ClientId, req.ClientSecret);
		TokenRetrievalResponse response = await command.ExecuteAsync(ct);
		return TypedResults.Ok(response);
	}
}