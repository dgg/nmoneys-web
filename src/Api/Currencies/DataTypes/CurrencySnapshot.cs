using System.ComponentModel.DataAnnotations;
using Mapster;
using NMoneys.Api.Infrastructure.OpenApi;

namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// Snapshot of currency information
/// </summary>
/// <param name="AlphabeticCode">3-Letter alphabetic code as per ISO 4217.</param>
/// <param name="EnglishName">Name of the currency, in English.</param>
/// <param name="IsObsolete">Indicates whether the currency is legal tender or it has been obsoleted.</param>
internal record CurrencySnapshot(string AlphabeticCode, string EnglishName, bool? IsObsolete) : IOpenApiSample<CurrencySnapshot>
{
	/// <example>CHF</example>
	[MinLength(3), MaxLength(3)]
	public string AlphabeticCode { get; } = AlphabeticCode;
	
	/// <example>Swiss Franc</example>
	[MinLength(1)]
	public string EnglishName { get; } = EnglishName;
	
	public static CurrencySnapshot Example { get; } = Currency.Chf.Adapt<CurrencySnapshot>();
}
