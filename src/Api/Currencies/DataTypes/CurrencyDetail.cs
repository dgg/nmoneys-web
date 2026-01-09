using System.ComponentModel.DataAnnotations;

using Mapster;

using NMoneys.Api.Infrastructure.OpenApi;

namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// Detailed currency information including formatting details.
/// </summary>
/// <param name="Code">ISO 4217 codes for the currency.</param>
/// <param name="Name">Names of the currency.</param>
/// <param name="Symbol">Currency symbol used in formatting.</param>
/// <param name="SignificantDigits">Number of significant decimal digits for the currency.</param>
/// <param name="IsObsolete">Indicates whether the currency is legal tender or it has been obsoleted.</param>
internal record CurrencyDetail(
	ExtendedCodes Code,
	CurrencyNames Name,
	string Symbol,
	int SignificantDigits,
	bool IsObsolete) : IOpenApiSample<CurrencyDetail>
{
	/// <summary>Currency symbol used in formatting.</summary>
	/// <example>CHF</example>
	[MinLength(1)]
	public string Symbol { get; } = Symbol;

	/// <summary>Number of significant decimal digits for the currency.</summary>
	/// <example>2</example>
	[Range(0, 5)]
	public int SignificantDigits { get; } = SignificantDigits;

	public static CurrencyDetail Example { get; } = Currency.Chf.Adapt<CurrencyDetail>();
}

/// <summary>
/// Extended ISO 4217 codes for the currency.
/// </summary>
internal record ExtendedCodes(string Alphabetic, ushort Numeric, string Padded) : SnapshotCodes(Alphabetic, Numeric)
{
	/// <summary>Zero-padded numeric code as per ISO 4217.</summary>
	/// <example>"756"</example>
	[MinLength(3), MaxLength(3)]
	public string Padded { get; } = Padded;
}
