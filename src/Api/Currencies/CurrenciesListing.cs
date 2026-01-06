using FastEndpoints;

using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Infrastructure.Serialization;
using NMoneys.Api.Infrastructure.Swagger;

namespace NMoneys.Api.Currencies;

/// <summary>
/// List of supported currencies (including obsolete ones).
/// </summary>
/// <param name="Currencies">Array of currency snapshots (including obsolete ones).</param>
internal sealed record CurrenciesListingResponse(CurrencySnapshot[] Currencies) : ISwaggerSample<CurrenciesListingResponse>
{
	public static CurrenciesListingResponse Example { get; } = new([CurrencySnapshot.Example]);
}

internal sealed class CurrenciesListing : EndpointWithoutRequest<CurrenciesListingResponse>
{
	public override void Configure()
	{
		Get("/currencies");
		AllowAnonymous();
		SerializerContext<SerializationContext>();
		Summary(s =>
		{
			s.Summary = "List currencies";
			s.Description = "Provides information about supported currencies. Obsolete currencies are returned.";
			s.Response(200, "List of currencies", example: CurrenciesListingResponse.Example);
		});
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		var command = new ListCurrencies();
		var response = await command.ExecuteAsync(ct);
		await Send.OkAsync(response, ct);
	}
}