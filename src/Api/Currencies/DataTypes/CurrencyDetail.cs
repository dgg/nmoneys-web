using System.ComponentModel.DataAnnotations;

using Mapster;

using NMoneys.Api.Infrastructure.OpenApi;

namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// Detailed currency information including formatting details.
/// </summary>
/// <param name="Codes">ISO 4217 codes for the currency.</param>
/// <param name="Names">Names of the currency.</param>
/// <param name="Symbol">Currency symbol used in formatting.</param>
/// <param name="SignificantDigits">Number of significant decimal digits for the currency.</param>
/// <param name="IsObsolete">Indicates whether the currency is legal tender or it has been obsoleted.</param>
internal record CurrencyDetail(
	CurrencyCodes Codes,
	CurrencyNames Names,
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
internal record CurrencyCodes(string Alphabetic, ushort Numeric, string Padded)
{
	/// <summary>3-Letter alphabetic code as per ISO 4217.</summary>
	/// <example>CHF</example>
	[MinLength(3), MaxLength(3)]
	public string Alphabetic { get; } = Alphabetic;

	/// <summary>Numeric code as per ISO 4217.</summary>
	/// <example>756</example>
	[Range(0, 999)]
	public ushort Numeric { get; } = Numeric;
	
	/// <summary>Zero-padded numeric code as per ISO 4217.</summary>
	/// <example>"756"</example>
	[MinLength(3), MaxLength(3)]
	public string Padded { get; } = Padded;
}

/// <summary>
/// Names of the currency.
/// </summary>
internal record CurrencyNames(string English, string Native)
{
	/// <summary>Name of the currency, in English.</summary>
	/// <example>Swiss Franc</example>
	[MinLength(1)]
	public string English { get; } = English;

	/// <summary>Name of the currency, in one of the languages of the countries/regions where the currency is used.</summary>
	/// <example>Schweizer Franken</example>
	[MinLength(1)]
	public string Native { get; } = Native;
}
