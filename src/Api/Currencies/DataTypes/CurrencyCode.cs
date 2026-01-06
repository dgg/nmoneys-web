using System.ComponentModel.DataAnnotations;

namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// ISO 4217 codes for the currency.
/// </summary>
internal record CurrencyCode(string Alphabetic, ushort Numeric)
{
	/// <summary>3-Letter alphabetic code as per ISO 4217.</summary>
	/// <example>CHF</example>
	[MinLength(3), MaxLength(3)]
	public string Alphabetic { get; } = Alphabetic;

	/// <summary>Numeric code as per ISO 4217.</summary>
	/// <example>756</example>
	[Range(0, 999)]
	public ushort Numeric { get; } = Numeric;
}
