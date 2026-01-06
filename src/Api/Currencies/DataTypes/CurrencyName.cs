namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// Names of the currency.
/// </summary>
internal record CurrencyName(string English, string Native)
{
	public static CurrencyName Map(Currency currency)
		=> new(currency.EnglishName, currency.NativeName);

	/// <summary>Name of the currency, in English.</summary>
	/// <example>Swiss Franc</example>
	public string English { get; } = English;

	/// <summary>Name of the currency, in one of the languages of the countries/regions where the currency is used.</summary>
	/// <example>Schweizer Franken</example>
	public string Native { get; } = Native;
}
