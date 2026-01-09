using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

internal sealed class CurrencyDetailMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencyDetail>()
			// no need to explicitly map Symbol or IsObsolete
			.Map(dest => dest.Code, src => src)
			.Map(dest => dest.Name, src => src)
			.Map(dest => dest.SignificantDigits, src => src.SignificantDecimalDigits);
	}
}