using FastEndpoints;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Infrastructure.Swagger;

namespace NMoneys.Api.Currencies;

// No request needed for this endpoint (EmptyRequest)

/// <summary>
/// Response for currencies listing
/// </summary>
/// <param name="Currencies">Array of currency snapshots</param>
internal sealed record CurrenciesListingResponse(CurrencySnapshot[] Currencies) : ISwaggerSample<CurrenciesListingResponse>
{
	public static CurrenciesListingResponse Example { get; } = new([
		CurrencySnapshot.Example
	]);
}

internal sealed class CurrenciesListing : EndpointWithoutRequest<CurrenciesListingResponse>
{
	public override void Configure()
	{
		Get("/currencies");
		AllowAnonymous();
		Summary(s =>
		{
			s.Summary = "List currencies";
			s.Description = "Provides information about supported currencies. Obsolete currencies are returned.";
			s.Response(200, "List of currencies", example: CurrenciesListingResponse.Example);
		});
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var snapshots = Currency.FindAll()
			.OrderBy(c => c.AlphabeticCode, StringComparer.Ordinal)
			.Take(5)
			.Select(CurrencySnapshot.Map)
			.ToArray();

		await Send.OkAsync(new CurrenciesListingResponse(snapshots), ct);
	}
}