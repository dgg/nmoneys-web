using Mapster;

namespace NMoneys.Api.Currencies.DataTypes;

internal sealed class CurrenciesMapping : IRegister
{
	/// <summary>
	/// Configure mapping
	/// </summary>
	public void Register(TypeAdapterConfig config)
	{
		// Currency -> CurrencyCode
		config.NewConfig<Currency, CurrencyCode>()
			.Map(dest => dest.Alphabetic, src => src.AlphabeticCode)
			.Map(dest => dest.Numeric, src => (ushort)src.NumericCode);

		// Currency -> CurrencyName
		config.NewConfig<Currency, CurrencyName>()
			.Map(dest => dest.English, src => src.EnglishName)
			.Map(dest => dest.Native, src => src.NativeName);

		// Currency -> CurrencySnapshot
		config.NewConfig<Currency, CurrencySnapshot>()
			.Map(dest => dest.Code, src => src)
			.Map(dest => dest.Name, src => src)
			.Map(dest => dest.IsObsolete, src => src.IsObsolete ? true : default(bool?));
	}
}