using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Infrastructure.Serialization;
using NMoneys.Api.Infrastructure.OpenApi;

namespace NMoneys.Api.Currencies;

/// <summary>
/// Request to retrieve currency details by ISO code.
/// </summary>
/// <param name="AlphabeticCode">ISO 4217 currency code.</param>
internal sealed record CurrencyRetrievalRequest(CurrencyIsoCode AlphabeticCode);

/// <summary>
/// Detailed currency information response.
/// </summary>
/// <param name="Currency">Detailed currency information.</param>
internal sealed record CurrencyRetrievalResponse(CurrencyDetail Currency) : IOpenApiSample<CurrencyRetrievalResponse>
{
	public static CurrencyRetrievalResponse Example { get; } = new(CurrencyDetail.Example);
}

internal sealed class CurrencyRetrieval : Endpoint<CurrencyRetrievalRequest, Results<Ok<CurrencyRetrievalResponse>, ProblemDetails>>
{
	public override void Configure()
	{
		Get("/currencies/{alphabeticCode:alpha}");
		AllowAnonymous();
		SerializerContext<SerializationContext>();
		Summary(s =>
		{
			s.Summary = "Get currency details";
			s.Description = "Retrieves detailed information about a specific currency by its ISO code.";
			s.Params["code"] = "ISO 4217 currency code (e.g., USD, EUR, CHF)";
			s.Response(200, "Currency details", example: CurrencyRetrievalResponse.Example);
		});
	}

	public override async Task<Results<Ok<CurrencyRetrievalResponse>, ProblemDetails>> ExecuteAsync(CurrencyRetrievalRequest req, CancellationToken ct)
	{
		var command = new RetrieveCurrency(req.AlphabeticCode);
		CurrencyRetrievalResponse response = await command.ExecuteAsync(ct);
		return TypedResults.Ok(response);
	}
}
