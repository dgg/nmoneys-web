using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

internal sealed class CurrencyDetailMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencyDetail>()
			// symbol and obsolete flag are auto-mapped
			.Map(dest => dest.Codes, src =>src)
			.Map(dest => dest.Names, src => src)
			.Map(dest => dest.SignificantDigits, src => src.SignificantDecimalDigits);
		
		config.NewConfig<Currency, CurrencyCodes>()
			.Map(dest => dest.Alphabetic, src => src.AlphabeticCode)
			.Map(dest => dest.Numeric, src => (ushort)src.NumericCode)
			.Map(dest => dest.Padded, src => src.PaddedNumericCode);
		
		config.NewConfig<Currency, CurrencyNames>()
			.Map(dest => dest.English, src => src.EnglishName)
			.Map(dest => dest.Native, src => src.NativeName);
	}
}