using System.ComponentModel.DataAnnotations;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using NMoneys.Api.Infrastructure.OpenApi;
using NMoneys.Api.Infrastructure.Serialization;
using NMoneys.Extensions;

namespace NMoneys.Api.Currencies;

/// <summary>
/// Request to format currency amounts.
/// </summary>
/// <param name="AlphabeticCode">ISO 4217 currency code.</param>
/// <param name="Amounts">Collection of amounts to format.</param>
internal sealed record AmountsFormattingRequest(
	CurrencyIsoCode AlphabeticCode,
	[property: MinLength(1)] decimal[] Amounts);

/// <summary>
/// Response containing formatted currency amounts.
/// </summary>
/// <param name="Formatted">Array of formatted currency strings.</param>
internal sealed record AmountsFormattingResponse(string[] Formatted) : IOpenApiSample<AmountsFormattingResponse>
{
	/// <summary>
	/// Array of formatted currency strings.
	/// </summary>
	[MinLength(1)]
	public string[] Formatted { get; } = Formatted;
	public static AmountsFormattingResponse Example { get; } = new([14.769m.Eur().ToString()]);
}

internal sealed class AmountsFormatting : Endpoint<AmountsFormattingRequest, Results<Ok<AmountsFormattingResponse>, ProblemDetails>>
{
	public override void Configure()
	{
		Post("/currencies/{alphabeticCode:alpha}/formats");
		AllowAnonymous();
		SerializerContext<SerializationContext>();
		Summary(s =>
		{
			s.Summary = "Format currency amounts";
			s.Description = "Formats a collection of amounts for a specific currency using the currency's formatting rules.";
			s.RequestParam(r => r.AlphabeticCode, "ISO 4217 currency code (e.g., USD, EUR, CHF)");
			s.Response(200, "Formatted amounts", example: AmountsFormattingResponse.Example);
		});
	}

	public override async Task<Results<Ok<AmountsFormattingResponse>, ProblemDetails>> ExecuteAsync(AmountsFormattingRequest req, CancellationToken ct)
	{
		var command = new FormatAmounts(req.AlphabeticCode, req.Amounts);
		AmountsFormattingResponse response = await command.ExecuteAsync(ct);
		return TypedResults.Ok(response);
	}
}
