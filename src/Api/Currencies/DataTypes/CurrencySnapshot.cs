using Mapster;

using NMoneys.Api.Infrastructure.OpenApi;

namespace NMoneys.Api.Currencies.DataTypes;

/// <summary>
/// Snapshot of currency information
/// </summary>
/// <param name="Code">ISO 4217 codes for the currency.</param>
/// <param name="Name">Names of the currency.</param>
/// <param name="IsObsolete">Indicates whether the currency is legal tender or it has been obsoleted.</param>
internal record CurrencySnapshot(CurrencyCode Code, CurrencyName Name, bool? IsObsolete) : IOpenApiSample<CurrencySnapshot>
{
	public static CurrencySnapshot Example { get; } = Currency.Chf.Adapt<CurrencySnapshot>();
}