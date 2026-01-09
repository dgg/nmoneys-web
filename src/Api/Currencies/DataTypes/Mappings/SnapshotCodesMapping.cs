using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

/// <summary>
/// Currency -> SnapshotCodes
/// </summary>
internal sealed class SnapshotCodesMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, SnapshotCodes>()
			.Map(dest => dest.Alphabetic, src => src.AlphabeticCode)
			.Map(dest => dest.Numeric, src => (ushort)src.NumericCode);
	}
}