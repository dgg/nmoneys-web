namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// ISO 4217 codes for the currency.
/// </summary>
internal record CurrencyCode(string Alphabetic, ushort Numeric)
{
	public static CurrencyCode Map(Currency currency)
		=> new(currency.AlphabeticCode, (ushort)currency.NumericCode);

	/// <summary>3-Letter alphabetic code as per ISO 4217.</summary>
	/// <example>CHF</example>
	public string Alphabetic { get; } = Alphabetic;

	/// <summary>Numeric code as per ISO 4217.</summary>
	/// <example>756</example>
	public ushort Numeric { get; } = Numeric;
}
