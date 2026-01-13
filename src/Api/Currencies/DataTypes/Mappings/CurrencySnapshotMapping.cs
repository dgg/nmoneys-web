using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

/// <summary>
/// Currency -> CurrencySnapshot
/// </summary>
internal sealed class CurrencySnapshotMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, CurrencySnapshot>()
			// name and code are auto-mapped
			.Map(dest => dest.IsObsolete, src => src.IsObsolete ? true : default(bool?));
	}
}