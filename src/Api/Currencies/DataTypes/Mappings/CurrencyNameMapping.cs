using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

/// <summary>
/// Currency -> CurrencyName
/// </summary>
internal sealed class CurrencyNameMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencyNames>()
			.Map(dest => dest.English, src => src.EnglishName)
			.Map(dest => dest.Native, src => src.NativeName);
	}
}