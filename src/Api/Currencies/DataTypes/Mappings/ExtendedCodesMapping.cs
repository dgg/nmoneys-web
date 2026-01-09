using Mapster;

namespace NMoneys.Api.Currencies.DataTypes.Mappings;

internal sealed class ExtendedCodesMapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Currency, ExtendedCodes>()
			.Map(dest => dest.Alphabetic, src => src.AlphabeticCode)
			.Map(dest => dest.Numeric, src => (ushort)src.NumericCode)
			.Map(dest => dest.Padded, src => src.PaddedNumericCode);
	}
}